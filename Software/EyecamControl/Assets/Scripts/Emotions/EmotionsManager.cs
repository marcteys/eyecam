using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;

public class EmotionsManager : MonoBehaviour
{

    public enum EmotionControlType
    {
        COMBINE,
        SINGLE,
    }

    [Header("Normal Mode")]
    public EmotionControlType controlType = EmotionControlType.SINGLE;

    public float defaultFadeDuration = 2;
    public bool useSingleEmotion = false;
    public float computeAverageSpeed = 2f;

    [Header("RANDOM Mode")]
    public float changeFrequency = 3;
    public float changeFrequencySD = 2;
    public KeyCode keyToStart;
    public bool isRandomEmotionPlaying = false;


    [Header("KEY Mode")]
    public float transitionKeyDuration = 2;


    [Header("Emotions")]
    public List<Emotion> emotions = new List<Emotion>();

    [Header("Settings")]
    public Toggle isSingleEmotionToggle;


    struct MotorsAngles {
        public float leftright;
        public float topbotton;
        public float topLid;
        public float bottomLid;
        public float leftEyeBrow;
        public float rightEyeBrow;

        public float GetAngleAt(int i)
        {
            if (i == 0) return leftright;
            if (i == 1) return topbotton;
            if (i == 2) return topLid;
            if (i == 3) return bottomLid;
            if (i == 4) return leftEyeBrow;
            else return rightEyeBrow;
        }
    };

    void Start()
    {
        foreach (Emotion emotion in emotions)
        {
            emotion.slider.button.onClick.AddListener(
                    delegate { TriggerEmotion(emotion, defaultFadeDuration); });
            emotion.slider.slider.onValueChanged.AddListener(
                delegate { SetEmotionIntensity(emotion, (float)emotion.slider.slider.value); });
            isSingleEmotionToggle.onValueChanged.AddListener(
                delegate { SetSingleEmotionSettings( isSingleEmotionToggle.isOn); });
        }
    }

    public void SetSingleEmotionSettings(bool newValue)
    {
        useSingleEmotion = newValue;
    }

    public void SetEmotionIntensity(Emotion emotion, float intensity)
    {
        //Turn off other Emotions
        if (useSingleEmotion)
        {
            foreach (Emotion e in emotions)
            {
                if (e != emotion && e.intensity != 0)
                {
                    e.SetIntensity(0, true);
                }
            }
        }

        //Set intensity
        emotion.intensity = intensity;
    }

    void Update()
    {
        if (EyeControl.Instance.controlMode == EyeControl.ControlMode.EMOTION)
            ComputeEmotionAverage();
        else if (EyeControl.Instance.controlMode == EyeControl.ControlMode.EMOTION_RANDOM)
        {
       //     useSingleEmotion = true;
            if (Input.GetKeyDown(keyToStart))
            {
                Debug.Log("Starting random emotion coroutine");
                if (!isRandomEmotionPlaying)
                {
                    StopCoroutine("RandomEmotion");
                    StartCoroutine("RandomEmotion");
                } else
                {
                    StopCoroutine("RandomEmotion");
                }
            }
            ComputeEmotionAverage();
        }
        else if (EyeControl.Instance.controlMode == EyeControl.ControlMode.EMOTION_KEY)
        {
           // useSingleEmotion = true;
            foreach (Emotion e in emotions)
            {
                if (Input.GetKeyDown(e.keyToTrigger))
                {
                    TriggerEmotion(e, transitionKeyDuration);
                    Debug.Log("Trigger Emotion " + e.name + " with keys");
                }
            }
            ComputeEmotionAverage();
        }
    }

    IEnumerator RandomEmotion()
    {
        while (true)
        {
            isRandomEmotionPlaying = false;
            float t = changeFrequency + Random.Range(-changeFrequencySD, changeFrequencySD);
            int rand = Random.Range(0, emotions.Count - 1);
            Debug.Log("Playing emotion  " + rand + " in " + t);
            yield return new WaitForSeconds(t);
            TriggerEmotion(emotions[rand], defaultFadeDuration);
            isRandomEmotionPlaying = true;
        }
    }

    void ComputeEmotionAverage()
    {







        MotorsAngles angles;
        angles.leftright = 0;
        angles.topbotton = 0;
        angles.topLid = 0;
        angles.bottomLid = 0;
        angles.leftEyeBrow = 0;
        angles.rightEyeBrow = 0;
        MotorsAngles totalAngles;
        totalAngles.leftright = 0;
        totalAngles.topbotton = 0;
        totalAngles.topLid = 0;
        totalAngles.bottomLid = 0;
        totalAngles.leftEyeBrow = 0;
        totalAngles.rightEyeBrow = 0;

        foreach (Emotion emotion in emotions)
        {
            if(emotion.intensity != 0)
            {
                if (emotion.leftright != -1) {
                    angles.leftright += emotion.leftright * emotion.intensity;
                    totalAngles.leftright += 1 * emotion.intensity;
                }

                if (emotion.topbotton != -1)
                {
                    angles.topbotton += emotion.topbotton * emotion.intensity;
                    totalAngles.topbotton += 1 * emotion.intensity;
                }

                if (emotion.topLid != -1)
                {
                    angles.topLid += emotion.topLid * emotion.intensity;
                    totalAngles.topLid += 1 * emotion.intensity;
                }

                if (emotion.bottomLid != -1)
                {
                    angles.bottomLid += emotion.bottomLid * emotion.intensity;
                    totalAngles.bottomLid += 1 * emotion.intensity;
                }

                if (emotion.leftEyeBrow != -1)
                {
                    angles.leftEyeBrow += emotion.leftEyeBrow * emotion.intensity;
                    totalAngles.leftEyeBrow += 1 * emotion.intensity;
                }

                if (emotion.rightEyeBrow != -1)
                {
                    angles.rightEyeBrow += emotion.rightEyeBrow * emotion.intensity;
                    totalAngles.rightEyeBrow += 1 * emotion.intensity;
                }
            }
        }



        for(int i = 0; i < 6; i++)
        {
            float target = 0;

            if (totalAngles.GetAngleAt(i) != 0)
            {
                float currentPos = EyeControl.Instance.GetMotorPosition(i);
                float finalTargetVal = angles.GetAngleAt(i) / totalAngles.GetAngleAt(i);
                target = Mathf.Lerp(currentPos, finalTargetVal, Time.deltaTime * computeAverageSpeed);
                EyeControl.Instance.SetMotorPosition(i, target, true);
            }

        }
     
    }


    public void TriggerEmotion(Emotion emotion, float fadeDuration)
    {
        //Turn off all emotions except this one
        bool delay = VerifyTurnOffEmotions(emotion, fadeDuration);

        
        //Trigger this emotion
        float target = emotion.slider.buttonIsOn ? 0 : 1; Debug.Log("TODO : quand on clique sur le bouton, ça doit s'inverser");
        StartCoroutine(Animate(emotion, fadeDuration, target, delay));
    }


    public bool VerifyTurnOffEmotions(Emotion emotion, float fadeDuration)
    {
        bool hasEmotionOff = false;
        if (useSingleEmotion)
        {
            foreach (Emotion e in emotions)
            {
                if (e != emotion && e.intensity != 0)
                {
                    StartCoroutine(Animate(e, fadeDuration, 0));
                    hasEmotionOff = true;
                }
            }
        }
        return hasEmotionOff;
    }

    /*
    public void TriggerEmotion(Emotion emotion, float fadeDuration, float target)
    {
        StartCoroutine(Animate(emotion, fadeDuration, target));
    }*/

    public void UpdateEmotion(Emotion emotion)
    {
        if (emotion.intensity == 0)
            return;
      /*  float leftRightT = EyeControl.Instance.GetMotorPosition(0);
      float topBottomT = EyeControl.Instance.GetMotorPosition(1);
      float lidTopT = EyeControl.Instance.GetMotorPosition(2);
      float lidBottomT = EyeControl.Instance.GetMotorPosition(3);
      float eyebrowLeftT = EyeControl.Instance.GetMotorPosition(4);
      float eyebrowRightT = EyeControl.Instance.GetMotorPosition(5);
      */
        float[] targetPositions = new float[6];
        targetPositions[0] = emotion.leftright;
        targetPositions[1] = emotion.topbotton;
        targetPositions[2] = emotion.topLid;
        targetPositions[3] = emotion.bottomLid;
        targetPositions[4] = emotion.leftEyeBrow;
        targetPositions[5] = emotion.rightEyeBrow;
        float[] updatedPosition = new float[6];
        for (int i = 0; i < 6; i++)
        {
            updatedPosition[i] = EyeControl.Instance.GetMotorPosition(i);
        }

        for (int i = 0; i < 6; i++)
        {
            updatedPosition[i] = Mathf.Lerp(updatedPosition[i], targetPositions[i], emotion.intensity);
            if (targetPositions[i] != -1)
                EyeControl.Instance.SetMotorPosition(i, updatedPosition[i]);
        }
    }

    public IEnumerator Animate(Emotion emotion, float fadeDuration, float target, bool delay = false)
    {
      //  if(delay)   yield return new WaitForSeconds(fadeDuration);

        float updatedPosition = emotion.intensity;
        var t = 0.0f;
        while (t < 1 && (Mathf.Abs(target - updatedPosition) > 0.01f))
        {
            t += Time.deltaTime / fadeDuration;
            updatedPosition = Mathf.Lerp(updatedPosition, target, t);
            Debug.Log(emotion.name + " " + updatedPosition);
            emotion.SetIntensity(updatedPosition, true);
            yield return new WaitForEndOfFrame();
        }
        emotion.SetIntensity(target, true);

   
    }

    public void TriggerEmotions(Emotion[] emotions, float[] fadeDurations)
    {

    }
}
