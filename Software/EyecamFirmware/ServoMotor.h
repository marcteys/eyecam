#include <VarSpeedServo.h>

class ServoMotor {


  public :
    void update();

    void attach();
    void detach();
    void reset();

    void setPosition(int newPosition);
    void setSpeed(int speed);

    ServoMotor(char* motorName, int pin, int minAngle, int maxAngle);

    VarSpeedServo  servo;

  private:
    void controlMotor();
    void checkForDetach();

    char* name = "";
    int pin;
    int minAngle;
    int maxAngle;

    int currentPosition;
    int targetAnglePosition;

    int currentSpeed = 0;

    unsigned long lastUpdate = 0;
    int detachAfterDelay = 5000;
    bool isAttached = false;


    unsigned long passedTime = 0;
};



ServoMotor::ServoMotor(char* motorName, int p, int min, int max) {
  name = motorName;
  pin = p;
  minAngle = min;
  maxAngle = max;
}

void ServoMotor::attach() {
  servo.attach(pin);
  delay(1);
}

void ServoMotor::detach() {
  servo.detach();
  pinMode(pin, LOW);
}

void ServoMotor::reset() {
  servo.write(90);
}



void ServoMotor::setPosition(int angle) {
  if (angle < minAngle) angle = minAngle;
  else if (angle > maxAngle) angle = maxAngle;

  targetAnglePosition = angle;
}

void ServoMotor::setSpeed(int speed) { }

void ServoMotor::update() {


  controlMotor();
  checkForDetach();

}

void ServoMotor::controlMotor() {

  if (currentPosition != targetAnglePosition) {

    if (!servo.attached())
      attach();


#if DEBUG
    Serial.print("Setting position of ");
    Serial.print(name);
    Serial.print(" to ");
    Serial.println(targetAnglePosition);
#endif
    // Write Position instant
    if (currentSpeed == 0) {
      servo.write(targetAnglePosition);
      currentPosition = targetAnglePosition;
      lastUpdate = millis();
      delayMicroseconds(10);
    }

    // Write Position with delay incremented every update
    else {
      Serial.println("Not implemented");
    }
  }

}

void ServoMotor::checkForDetach() {


  if (detachAfterDelay != 0 ) {
    // Ready to be detached
    if (servo.attached() && currentPosition == targetAnglePosition) {

      if (millis() - lastUpdate > detachAfterDelay) {
        servo.detach();
#if DEBUG
        Serial.print("Auto detaching servo ");
        Serial.println(name);
#endif
      }

    }

  }

}
