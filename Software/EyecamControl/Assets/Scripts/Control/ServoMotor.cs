using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

[System.Serializable]
public class ServoMotor 
{
    public enum RotationDirection
    {
        CLOCKWISE,
        COUNTER_CLOCKWISE
    };
    public enum Axis
    {
        X,
        Y,
        Z,
        NONE
    };

    [HideInInspector]
    public string name = "";

    [Header("Motor Control")]
    public RotationDirection rotationDirection;
    [Range(0, 1)]
    public float currentPosition = 0.5f;

    int currentPositionAngle = 90;
    public int minAngle = 90;
    public int maxAngle = 90;

    //TODO : this are useless
     int realLifeMinAngle = 90;
     int realLifeMaxAngle = 90;

    [Header("Visual Control")]
    public SliderComponent attachedSlider;
    public Transform simulatorObject;
    public Axis similatorObjectAxis;
    public Vector2 simulatorMinMaxAngle;
    public bool changeAngle180 = false;

    // Start is called before the first frame update
    public void init()
    {
        attachedSlider.slider.onValueChanged.AddListener(delegate { SetPosition((float)attachedSlider.slider.value, false); });
    }


    public void SetPosition(float sliderValue, bool updateSlider = true)
    {
        // sliderValue = Mathf.Clamp(sliderValue, 0, 1);
        //https://answers.unity.com/questions/953142/ui-slider-how-to-tell-if-value-changed-by-user-rat.html
        if (currentPosition != sliderValue)
        {
            currentPosition = sliderValue;
            if (updateSlider)
                UpdateSliderValue();
        }
    }

    void UpdateSliderValue()
    {
     //   Debug.Log(EventSystem.current.currentSelectedGameObject);

            /*
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            UnityEngine.UI.InputField IF = EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.InputField>();
            if (IF != null)
            {
                return true;
            }
        }
        return false;
        */
    //    Debug.Log(attachedSlider.slider.GetComponent<SliderUIUserSelected>().IsUserTriggerd);
       attachedSlider.slider.value = currentPosition;
    }

    public void SimulatePosition()
    {
        if(simulatorObject != null)
        {
            float remappedAngle = currentPosition.Map(0.0f,1.0f, simulatorMinMaxAngle.x, simulatorMinMaxAngle.y);
            Vector3 targetAngle = simulatorObject.localEulerAngles;
            switch (similatorObjectAxis)
            {
                case Axis.X:
                    targetAngle.x = remappedAngle;
                    break;
                case Axis.Y:
                    targetAngle.y = remappedAngle;
                    break;
                case Axis.Z:
                    targetAngle.z = remappedAngle;
                    break;
                case Axis.NONE:
                    break;
            }

            simulatorObject.localEulerAngles = targetAngle;

        }
    }

    public void SimulatedPositionToAngle()
    {
        //Get the position and change it to Euler Angle
        float pos = 0;

        switch (similatorObjectAxis)
        {
            case Axis.X:
                pos = simulatorObject.localEulerAngles.x;
                break;
            case Axis.Y:
                pos = simulatorObject.localEulerAngles.y;
                break;
            case Axis.Z:
                pos = simulatorObject.localEulerAngles.z;
                break;
            case Axis.NONE:
                break;
        }
        if (changeAngle180) pos = ChangeAngleDisplayAngles(pos);

        float posInPercent = pos.Map(simulatorMinMaxAngle.x, simulatorMinMaxAngle.y, 0.0f, 1.0f);
        currentPosition = posInPercent;

        UpdateSliderValue();
    }

    /* public void ObjectToAngle()
     {

         float lr = eye.transform.localEulerAngles.y;
         float tb = ChangeAngleDisplayAngles(eye.transform.localEulerAngles.x);

         float mappedLR = lr.Map(130.0f, 233.0f, 0.0f, 1.0f);
         EyeControl.Instance.SetMotor(0, mappedLR);

         float mappedTB = tb.Map(-50.0f, 50.0f, 0.0f, 1.0f);
         EyeControl.Instance.SetMotor(1, mappedTB);
     }*/

    // Change 0 - 360 angles to -180 / 180
    float ChangeAngleDisplayAngles(float a)
    {
        if (a > 180f)
            a -= 360f;

        return a;
    }



    public int RelativePositionToAngle()
    {
      //  currentPositionAngle = Mathf.RoundToInt(currentPosition.Map(0, 1, minAngle, maxAngle));

       
        if(rotationDirection == RotationDirection.CLOCKWISE)
            currentPositionAngle  = Mathf.RoundToInt(currentPosition.Map(0, 1, minAngle, maxAngle));
        else if (rotationDirection == RotationDirection.COUNTER_CLOCKWISE)
            currentPositionAngle = Mathf.RoundToInt(currentPosition.Map(0, 1, maxAngle, minAngle ));
          
        return currentPositionAngle;
    }
}
