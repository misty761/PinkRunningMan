using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour
{
    public GameObject imageMusicOn;
    public GameObject imageMusicOff;

    bool bMusicOn;
    AudioSource music;
    
    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();   
        int musicState = PlayerPrefs.GetInt("MusicState", 1);
        if (musicState != 0) 
        {
            bMusicOn = true;
        } 
        else 
        {
            bMusicOn = false;
        }
        SetMusic();

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);
    }

    private void TouchUp()
    {
        bMusicOn = !bMusicOn;
        SetMusic();
    }

    void SetMusic()
    {
        if (bMusicOn)
        {
            music.Play();
            imageMusicOn.SetActive(true);
            imageMusicOff.SetActive(false);
            PlayerPrefs.SetInt("MusicState", 1);    // 0: music off, 1: music on
        }
        else{
            music.Stop();
            imageMusicOn.SetActive(false);
            imageMusicOff.SetActive(true);
            PlayerPrefs.SetInt("MusicState", 0);    // 0: music off, 1: music on
        }
    }

}
