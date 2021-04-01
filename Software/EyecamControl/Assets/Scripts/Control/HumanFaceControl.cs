#if UDUINO_READY

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanFaceControl : MonoBehaviour
{

    public GameObject cameraPlane = null;

    FaceDetectRobust faceDetection = null;
    [Range(0,1)]
    public float percentX;
    [Range(0, 1)]
    public float percentY;

    [Range(0, 1)]
    public float targetX = 0.5f;
    [Range(0, 1)]
    public float targetY = 0.5f;

    public float lerpSpeed = 20;

    public bool shyMode = false;

    // Start is called before the first frame update
    void Start()
    {
        targetX = EyeControl.Instance.GetMotorPosition(0);
        targetY = EyeControl.Instance.GetMotorPosition(1);
        if (EyeControl.Instance.controlMode == EyeControl.ControlMode.FOLLOW_HUMAN)
            cameraPlane.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {


        if (EyeControl.Instance.controlMode == EyeControl.ControlMode.FOLLOW_HUMAN)
        {
            if (faceDetection == null)
                faceDetection = FaceDetectRobust.Instance;


            if (faceDetection.isPresent)
            {
                percentX = faceDetection.posX / 1280.0f;
                percentY = faceDetection.posY / 720.0f;

                if(shyMode)
                {
                    EyeControl.Instance.SetMotorPosition(0, 1.0f - percentX, true);
                    EyeControl.Instance.SetMotorPosition(1, 1.0f - percentY, true);
                } else
                {
                    targetX = Mathf.Lerp(targetX, percentX, Time.deltaTime * lerpSpeed);
                    targetY = Mathf.Lerp(targetY, percentY, Time.deltaTime * lerpSpeed);

                    if (Mathf.Abs(targetX - 0.5f) > 0.1f) EyeControl.Instance.SetMotorPosition(0, 1.0f - targetX, true);
                    if (Mathf.Abs(targetY - 0.5f) > 0.1f) EyeControl.Instance.SetMotorPosition(1, targetY, true);
                }
            }
        }

    }
}
#endif