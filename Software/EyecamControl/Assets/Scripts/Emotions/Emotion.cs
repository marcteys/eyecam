using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Emotion 
{
   // [HideInInspector]
    public string name = "";

    [Header("CurrentParameters")]
    [Range(0, 1)]
    public float intensity = 0.0f;


    [Header("StaticValues")]
    public float leftright = -1;
    public float topbotton = -1;
    public float topLid = -1;
    public float bottomLid = -1;
    public float leftEyeBrow = -1;
    public float rightEyeBrow = -1;

    [Header("GameObject")]
    public SliderComponent slider;


    [Header("Trigger")]
    public KeyCode keyToTrigger;


    public Emotion()
    {
        if(slider != null)
        slider.slider.onValueChanged.AddListener(
            delegate { SetIntensity((float)slider.slider.value, false); }
            );
    }

    public void SetIntensity(float newIntensity, bool updateSlider = true)
    {
        intensity = newIntensity ;

        if(updateSlider)
            UpdateSliderValue();
    }

    public void UpdateSliderValue()
    {
        slider.slider.value = intensity;
    }
}




