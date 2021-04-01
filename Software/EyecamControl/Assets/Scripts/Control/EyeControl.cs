

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UDUINO_READY
    using Uduino;
#endif


public class EyeControl : MonoBehaviour
{
    public enum ControlMode
    {
        REALTIME,
        EMOTION,
        EMOTION_RANDOM,
        EMOTION_KEY,
        FOLLOW_CURSOR,
        FOLLOW_HUMAN,
        SCENARIO
    }

    [HideInInspector]
    public static EyeControl Instance =  null;
    public ControlMode controlMode = ControlMode.REALTIME; // TODO : this is unused
    public bool simulatePosition = true;

    [Header("Control Slider")]
    [Range(0,1)]
    public float EyeballLeftRight = 0.5f;
    [Range(0, 1)]
    public float EyeballTopBottom = 0.5f;


    [Header("Setup")]
    public List<ServoMotor> motors = new List<ServoMotor>();


    /// Addition functions
    /// 
    [Header("Blink")]
    public bool blink = false;
    public float blinkFrequency = 3.8f; // 18 times per seconds
    public float blinkFrequencySD = 1.5f;
    public float blinkDuration = 0.1f;
    public bool isBlinking = false;
    public bool blinkOverrideEverything = true;


    [Header("Pupille Procedural")]
    public bool generateSaccade = false;
    public float saccadeVariance = 0.2f;
    public bool lookRandom = false;
    bool isLookingRandom = false;
    public float lookRandomVariance = 0.2f;
    public float lookRandomFrequency = 0.2f;
    public float lookRandomFrequencySD = 0.2f;
    public bool avoidLookingCenter = false;
    // public float lookRandomVariance = 0.2f;


    [Header("EyeLid")]
    public bool adjustLidBasedOnGaze = false;
    [Range(0, 1)]
    public float EyelidOpenClose = 0.5f;
    [Range(0, 1)]
    public float EyeballRotationInfluenceFactor = 0.5f;
    [Range(0, 0.5f)]
    public float EyeballInfluenceAfter = 0.25f;


    [Header("Mouse Control")]
    public bool mapScrollWheelOnLid = true;
    public bool setEyebrowsLeftCLick = true;


    [Header("UI")]
    public GameObject[] UIObjects;
    bool isUIDisplay = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("DEstroying");
            Destroy(this);
        }
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(ServoMotor motor in motors)
        {
            motor.init();
        }
    }


    public void SetMotorPosition(float a, float b, float c, float d, float e, float f,bool updateSlider = true)
    {
        SetMotorPosition(0, a, updateSlider);
        SetMotorPosition(1, b, updateSlider);
        SetMotorPosition(2, c, updateSlider);
        SetMotorPosition(3, d, updateSlider);
        SetMotorPosition(4,e, updateSlider);
        SetMotorPosition(5, f, updateSlider);
    }



    public void SetMotorPosition(int index, float anglePercent, bool updateSlider = true)
    {
        if((index == 0 || index == 1) && generateSaccade)
        {
            anglePercent = GenerateSaccade(anglePercent);
        }
        if ((index == 2 || index == 3) && blinkOverrideEverything && isBlinking)
        {
          //  Debug.Log("Do nothing, it's blinking");
        }
        else
        {
            motors[index].SetPosition(anglePercent, updateSlider);
        }
    }

    public float GetMotorPosition(int index)
    {
        return motors[index].currentPosition;
    }

    // Update is called once per frame
    void Update()
    {




        if (controlMode != ControlMode.EMOTION && controlMode != ControlMode.REALTIME && controlMode != ControlMode.EMOTION_RANDOM && 
            controlMode != ControlMode.SCENARIO)
            if (adjustLidBasedOnGaze && !isBlinking ) AdjustLidBasedOnGaze();



        ManualControl();
     

        if (blink)
        {
            Debug.Log("StartBlinkCoroutine");
            StopCoroutine("Blink");
            StartCoroutine("Blink");
            blink = false;
        }



        if (lookRandom)
        {
            Debug.Log("StartLookRandomCoroutine");
            StopCoroutine("LookRandom");
            StartCoroutine("LookRandom");
            lookRandom = false;
        }



        if (simulatePosition)
        {
            foreach (ServoMotor motor in motors)
            {
                motor.SimulatePosition();
            }
        }
    }

    void ManualControl()
    {
        // MOUSECONTROL
        if (mapScrollWheelOnLid)
        {
            if (!Input.GetMouseButton(2))
            {
                float a = Input.GetAxis("Mouse ScrollWheel");
                EyelidOpenClose += a / 3.0f;
            }
        }

        if (setEyebrowsLeftCLick)
        {
            if (Input.GetMouseButton(2))
            {
                float a = Input.GetAxis("Mouse ScrollWheel");
                float t = a / 5.0f;

                SetMotorPosition(4, GetMotorPosition(4) + t, true);
                SetMotorPosition(5, GetMotorPosition(5) + t, true);
            }
        }


        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StopCoroutine("LookRandom");
            SetMotorPosition(0, 0.5f, true);
            SetMotorPosition(1, 0.5f, true);
        }


    }

    float GenerateSaccade(float inValue)
    {
        return inValue + Random.Range(-saccadeVariance, saccadeVariance);
    }

    void AdjustLidBasedOnGaze()
    {
        // 0.2 : top: 0.7 Bottol : 0

        float eyeAngle = motors[1].currentPosition;
        float eyeLidBottom = eyeAngle;
        float eyeLidTop = eyeAngle;
        eyeLidTop = (1 - eyeLidTop.Map(0f, 0.5f, 0f, 1f)) * EyeballRotationInfluenceFactor  + EyelidOpenClose ;
        eyeLidBottom = eyeLidBottom.Map(0.5f, 1.0f, 0f, 1f) * EyeballRotationInfluenceFactor + EyelidOpenClose ;

        motors[2].SetPosition(eyeLidTop);
        motors[3].SetPosition(eyeLidBottom);
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkFrequency + Random.Range(-blinkFrequencySD, blinkFrequencySD));
            isBlinking = true;
            float previousTopLidPosition = motors[2].currentPosition;
            float previousBottomLidPosition = motors[3].currentPosition;
            motors[2].SetPosition(0.0f);
            motors[3].SetPosition(0.0f);
            yield return new WaitForSeconds(( blinkDuration + Random.Range(-0.1f, 0.1f) ) );
            motors[2].SetPosition(previousTopLidPosition);
            motors[3].SetPosition(previousBottomLidPosition);
            isBlinking = false;
        }
    }


    IEnumerator LookRandom()
    {
        while (true)
        {

        //    for(int i = 0; i < 5;i++)
            {
                isLookingRandom = false;

                yield return new WaitForSeconds(lookRandomFrequency + Random.Range(-lookRandomFrequencySD, lookRandomFrequencySD));
                isLookingRandom = true;
                float newLR = motors[0].currentPosition + Random.Range(-lookRandomVariance,lookRandomVariance);
                float newTD = motors[1].currentPosition + Random.Range(-lookRandomVariance, lookRandomVariance);
                if(avoidLookingCenter)
                {
                    while(newLR.IsBetweenRange(0.4f,0.6f))
                    {
                        newLR = motors[0].currentPosition + Random.Range(-lookRandomVariance, lookRandomVariance);

                    }
                    while (newTD.IsBetweenRange(0.4f, 0.6f))
                    {
                        newTD = motors[1].currentPosition + Random.Range(-lookRandomVariance, lookRandomVariance);
                    }
                }
                if (newLR < 0.2f) newLR = 0.2f;
                else if (newLR > 0.8f) newLR = 0.8f;
                if (newTD < 0.2f) newTD = 0.2f;
                else if (newTD > 0.8f) newTD = 0.8f;
                SetMotorPosition(0, newLR, true);
                SetMotorPosition(1, newTD, true);
            }
       //     yield return new WaitForSeconds(lookRandomFrequency + Random.Range(-lookRandomFrequencySD, lookRandomFrequencySD));

          //  SetMotorPosition(0, previousLR);
           // SetMotorPosition(1, previousLR);

        }
    }


    public void ToggleUI()
    {
        isUIDisplay = !isUIDisplay;
         foreach (GameObject g in UIObjects)
        {
            g.SetActive(isUIDisplay);
        }
    }
}


