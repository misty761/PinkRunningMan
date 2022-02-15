using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject imageSoundOn;
    public GameObject imageSoundOff;
    public AudioClip audioClick;
    public static bool bSoundOn;

    private AudioSource audioButton;
    void Start()
    {
        audioButton = GetComponent<AudioSource>();
        audioButton.clip = audioClick;

        int soundState = PlayerPrefs.GetInt("SoundState", 1);
        if (soundState != 0) 
        {
            bSoundOn = true;
        } 
        else 
        {
            bSoundOn = false;
        }
        SetSound();

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);
    }

    private void SetSound()
    {
        if (bSoundOn)
        {
            imageSoundOn.SetActive(true);
            imageSoundOff.SetActive(false);
            PlayerPrefs.SetInt("SoundState", 1);    // 0: music off, 1: music on
        }
        else{
            imageSoundOn.SetActive(false);
            imageSoundOff.SetActive(true);
            PlayerPrefs.SetInt("SoundState", 0);    // 0: music off, 1: music on
        }
    }

    private void TouchUp()
    {
        bSoundOn = !bSoundOn;
        SetSound();

        if (bSoundOn)
        {
            audioButton.Play();
        } 
    }
}
