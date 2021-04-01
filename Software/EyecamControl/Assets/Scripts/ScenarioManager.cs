using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{

    public string scenarioName;
    public KeyCode triggerKey;


    public bool playModeLoop = false;

    public Animator anim;


    public float pupilleLR = 0.5f;
    public float pupilleTB = 0.5f;
    public float eyelidUp = 0.5f;
    public float eyelidBot = 0.5f;
    public float eyeBrowLeft = 0.5f;
    public float eyeBrowRight = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EyeControl.Instance.controlMode == EyeControl.ControlMode.SCENARIO)
        {
            if (Input.GetKeyDown(triggerKey))
            {

                if (anim != null)
                {
                    anim.Play("Base Layer." + scenarioName);
                }

            }
            if (AnimatorIsPlaying(scenarioName)) {
                EyeControl.Instance.SetMotorPosition(pupilleLR, pupilleTB, eyelidUp, eyelidBot, eyeBrowLeft, eyeBrowRight, true);
            } else
            {
                if(playModeLoop)
                {
                    anim.Play("Base Layer." + scenarioName);

                }
            }
        }
    }



    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }


    bool AnimatorIsPlaying()
    {
        return anim.GetCurrentAnimatorStateInfo(0).length >
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
