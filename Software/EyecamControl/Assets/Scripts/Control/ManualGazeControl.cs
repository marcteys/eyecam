using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualGazeControl : MonoBehaviour
{

    public GameObject eye;
    public Camera camera;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if (EyeControl.Instance.controlMode == EyeControl.ControlMode.FOLLOW_CURSOR)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                target.position = hit.point;
                // Do something with the object that was hit by the raycast.
                eye.transform.LookAt(target);

                EyeControl.Instance.motors[0].SimulatedPositionToAngle();
                EyeControl.Instance.motors[1].SimulatedPositionToAngle();

            }
        }
    }

}
