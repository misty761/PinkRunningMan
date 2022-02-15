using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LeaderBoard : MonoBehaviour
{
    public AudioClip audioClick;

    AudioSource audioButton;
    string LEADER_BOARD_ID = "Cxxxxxxxxxxxxxx";

    private void Awake()
    {
        // Google play games
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioButton = GetComponent<AudioSource>();
        audioButton.clip = audioClick;

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_TouchDown = new EventTrigger.Entry();
        entry_TouchDown.eventID = EventTriggerType.PointerDown;
        entry_TouchDown.callback.AddListener((data) => { TouchDown(); });
        eventTrigger.triggers.Add(entry_TouchDown);

        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);

        LogInPlayGames();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogInPlayGames()
    {
        //이미 인증된 사용자는 바로 로그인 성공됩니다.
        if (Social.localUser.authenticated)
        {
            Debug.Log(Social.localUser.userName);
            //txtLog.text = "name : " + Social.localUser.userName + "\n";
        }
        else
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log(Social.localUser.userName);
                    //txtLog.text = "name : " + Social.localUser.userName + "\n";
                }
                else
                {
                    Debug.Log("Login Fail");
                    //txtLog.text = "Login Fail\n";

                    PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) => {
                        // handle results
                        Debug.Log("handle results : " + result);
                    });
                }
            });
    }


    public void TouchDown()
    {
        if (SoundControl.bSoundOn) audioButton.Play();
    }

    public void TouchUp()
    {
        RankButtonClick();
    }

    public void RankButtonClick()
    {
        Social.localUser.Authenticate(AuthenticateHandler);
    }

    void AuthenticateHandler(bool isSuccess)
    {
        if (isSuccess)
        {
            int highScore = PlayerPrefs.GetInt("BestScore");
            Social.ReportScore((long) highScore, LEADER_BOARD_ID, (bool success) =>
            {
                if (success)
                {
                    PlayGamesPlatform.Instance.ShowLeaderboardUI(LEADER_BOARD_ID);
                    Debug.Log("Show Leader Board UI : " + success);
                    Debug.Log("highScore : " + highScore);
                }
                else
                {
                    Debug.Log("Show Leader Board UI : " + success);
                }
            });
        }
        else
        {
            // login failed
            Debug.Log("Login failed to Google Play Games : " + isSuccess);
        }
    }
}
