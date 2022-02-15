using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour
{
    public AudioClip audioClick;
    public Text textRestart;

    AudioSource audioButton;
    // Start is called before the first frame update
    void Start()
    {
        audioButton = GetComponent<AudioSource>();
        audioButton.clip = audioClick;

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);
    }

    private void TouchUp()
    {
        if (GameManager.instance.isGameover && textRestart.IsActive())
        {
            //SceneManager.LoadScene("Main");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            if (SoundControl.bSoundOn) audioButton.Play();
        }
    }
}
