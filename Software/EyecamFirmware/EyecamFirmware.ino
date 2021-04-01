#include <Uduino.h> //  Dependency : https://assetstore.unity.com/packages/tools/input-management/uduino-arduino-and-unity-communication-simple-fast-and-stable-78402

#define DEBUG 1

#include "ServoMotor.h"

#define NUM_SERVOS 6

ServoMotor eyeServos[] = {
  //               name          pin    minAngle   maxAngle 
  ServoMotor("eyeball-leftright",   2,      55,        125),
  ServoMotor("eyeball-topdown",     3,      65,        130),
  ServoMotor("eyelid-top",        4,      80,        140),
  ServoMotor("eyelid-bottom",     5,      80,        150),
  ServoMotor("eyebrow-left",      6,      50,        140),
  ServoMotor("eyebrow-right",     7,      50,        140)
};

Uduino uduino("eye");


void setup() {
  Serial.begin(9600);
  /*
    for (int i = 0; i < NUM_SERVOS; i++) {
      eyeServos[i].init();
    }*/


  //  uduino.addCommand("sendTarget", );

  uduino.addCommand("ctrl", controlMotor);
  uduino.addCommand("rst", resetMotors);
  uduino.addCommand("stop", stopMotors);

  delay(10);
}


void loop() {
  uduino.update();
  if (uduino.isConnected()) {
    for (int i = 0; i < NUM_SERVOS; i++) {
      eyeServos[i].update();
    }
  }
}





void controlMotor() {
  int numPara = uduino.getNumberOfParameters();
#if DEBUG
  Serial.print("Control motor. ");
  Serial.print(numPara);
  Serial.print(" params. ");
#endif
  // If two parameters are sent
  if (numPara == 2 ) {
    char * valId = uduino.getParameter(0);
    int servo = uduino.charToInt(valId);

    char * valAngle = uduino.getParameter(1);
    int angle = uduino.charToInt(valAngle);
    if (angle != -1)
      eyeServos[servo].setPosition(angle);

  } else  if (numPara == 6 ) {

    // If all the servo are sent
    for (int i = 0; i < NUM_SERVOS; i++) {
      char * val = uduino.getParameter(i);
      int d = uduino.charToInt(val);
      if (d != -1)
        eyeServos[i].setPosition(d);
    }
  }

}

void resetMotors() {
  for (int i = 0; i < NUM_SERVOS; i++) {
    eyeServos[i].reset();
  }
}

void stopMotors() {
  for (int i = 0; i < NUM_SERVOS; i++) {
    eyeServos[i].detach();
  }
}
