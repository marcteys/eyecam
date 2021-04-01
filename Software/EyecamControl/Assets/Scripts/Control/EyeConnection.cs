
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UDUINO_READY
    using Uduino;
#endif

public class EyeConnection : MonoBehaviour
{
    [HideInInspector]
    public static EyeConnection Instance = null;
    [Header("Message")]
    public string lastMessage = "";
   
    [Header("Settings")]
    public bool alwaysSend = true;
    public float intervalBetweenUpdates = 0.016f;
    float timeElapsed;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= intervalBetweenUpdates)
        {
            timeElapsed -= intervalBetweenUpdates;
            CustomUpdate();
        }
    }

    // TODO : Faire ça ça veut dire qu'on peut pas envoyer a l'unité pendant l'update
    void CustomUpdate()
    {

        #if UDUINO_READY

        if (alwaysSend)
        {
            string angles = UduinoManager.BuildMessageParameters(
               EyeControl.Instance.motors[0].RelativePositionToAngle(),
               EyeControl.Instance.motors[1].RelativePositionToAngle(),
               EyeControl.Instance.motors[2].RelativePositionToAngle(),
               EyeControl.Instance.motors[3].RelativePositionToAngle(),
               EyeControl.Instance.motors[4].RelativePositionToAngle(),
               EyeControl.Instance.motors[5].RelativePositionToAngle()
            );

            if (angles != lastMessage)
            {
                UduinoManager.Instance.sendCommand("ctrl", angles);
                lastMessage = angles;
            }
        }
        #endif

    }

    public void ResetAll()
    {
        // send all angles to 90 en absolu
    }
}


