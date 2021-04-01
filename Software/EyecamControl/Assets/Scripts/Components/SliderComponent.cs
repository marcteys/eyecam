using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;


public class SliderComponent : MonoBehaviour
{
    public Text title;
    public Slider slider;
    public Text textValue;
    public Button button;
    public bool buttonIsOn = false;


    private void Awake()
    {
           //TODO : supprimer ce script
        // slider.gameObject.AddComponent<SliderUIUserSelected>();
    }


    public void ChangeButtonState()
    {
        if (button == null) return;
        if(buttonIsOn)
        {
            button.image.color = new Color(233.0f / 255.05f, 233.0f / 255.05f, 233.0f/255.05f);
            buttonIsOn = false;
        } else
        {
            buttonIsOn = true;
            button.image.color = new Color(76.0f / 255.05f, 76 / 255.05f, 76 / 255.05f);
        }
    }

    public void Update()
    {
        if (slider != null && slider.value != 0 && !buttonIsOn) ChangeButtonState();
        else if (slider != null && slider.value == 0 && buttonIsOn) ChangeButtonState();
    }


    public void SetTitle(string newTitle)
    {
        title.text = newTitle;
    }

    public void SetTextValue()
    {
        textValue.text = slider.value.ToString("F2");
    }
}
