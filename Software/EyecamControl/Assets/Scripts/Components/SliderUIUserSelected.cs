using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class SliderUIUserSelected : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler , IPointerDownHandler, ISelectHandler
{
    // Start is called before the first frame update
    private bool isUserTriggerd;

    public bool IsUserTriggerd
    {
        get { return isUserTriggerd; }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isUserTriggerd = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isUserTriggerd = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isUserTriggerd = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isUserTriggerd = false;
    }


    public void OnSelect(BaseEventData eventData)
    {
        isUserTriggerd = true;
    }

}
