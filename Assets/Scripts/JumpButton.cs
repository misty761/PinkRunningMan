using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour
{
    public static bool isTouching;

    // Start is called before the first frame update
    void Start()
    {
        isTouching = false;

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_TouchDown = new EventTrigger.Entry();
        entry_TouchDown.eventID = EventTriggerType.PointerDown;
        entry_TouchDown.callback.AddListener((data) => { TouchDown(); });
        eventTrigger.triggers.Add(entry_TouchDown);

        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);
    }

    private void TouchDown()
    {
        isTouching = true;
        Debug.Log("isTouching : " + isTouching);
    }

    private void TouchUp()
    {
        isTouching = false;
        Debug.Log("isTouching : " + isTouching);
    }
}
