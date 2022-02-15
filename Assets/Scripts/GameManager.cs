using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour {
    public static GameManager instance; // 싱글톤을 할당할 전역 변수
    public bool isGameover = false;     // 게임 오버 상태
    public Text scoreText;              // 점수를 출력할 UI 텍스트
    public Text bestScoreText;          // 최고 점수를 출력할 UI 텍스트
    public GameObject gameoverUI;       // 게임 오버시 활성화 할 UI 게임 오브젝트
    public float randomObstacle = 0f;   // 장애물이 점점 더 자주 나오도록하기 위해 사용
    public AudioClip audioClip;         // 기록 갱신시 재생할 소리
    public AudioSource audioFanfare;
    public GameObject goTitle;
    public GameObject goStartButton;

    int score = 0; // 게임 점수

    GoogleMobileAdsScript adMob;

    public enum State
    {
        Title,
        Play,
        GameOver
    }

    public State state;

    // 게임 시작과 동시에 싱글톤을 구성
    void Awake() 
    {
        // 싱글톤 변수 instance가 비어있는가?
        if (instance == null)
        {
            // instance가 비어있다면(null) 그곳에 자기 자신을 할당
            instance = this;

            
        }
        else
        {
            // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우

            // 씬에 두개 이상의 GameManager 오브젝트가 존재한다는 의미.
            // 싱글톤 오브젝트는 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        // Google AdMob
        adMob = GetComponent<GoogleMobileAdsScript>();
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        adMob.RequestInterstitial();

        audioFanfare = GetComponent<AudioSource>();

        state = State.Title;
        Time.timeScale = 0f;
        goTitle.SetActive(true);
        goStartButton.SetActive(true);
    }

    void Update() 
    {
        /*
        if (isGameover && Input.GetMouseButtonDown(0))
        {
            // 게임 오버 상태에서 마우스 왼쪽 버튼을 클릭하면 현재 씬 재시작
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        */
        if (!isGameover && score >= 10) GetAchievement();

        randomObstacle += Time.deltaTime * 0.005f;
        if (randomObstacle > 0.666f) randomObstacle = 0.666f;

        if (isGameover && !(audioFanfare.isPlaying || PlayerController.playerAudio.isPlaying))
        {
            // AdMob 광고 표시
            if (adMob.interstitial.IsLoaded())
            {
                adMob.interstitial.Show();
            }

            // 게임 오버 UI 표시
            gameoverUI.SetActive(true);

            state = State.GameOver;
            Time.timeScale = 0f;
        }
    }

    public void StartGame()
    {
        goTitle.SetActive(false);
        goStartButton.SetActive(false);
        state = State.Play;
        Time.timeScale = 1f;
    }

    public void GetAchievement()
    {
        if (score >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_trainee, 100f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Get Achievement");
                }
                else
                {
                    Debug.Log("Fail to get Achievement");
                }
            });
        }

        if (score >= 20)
        {
            Social.ReportProgress(GPGSIds.achievement_beginner, 100f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }

        if (score >= 30)
        {
            Social.ReportProgress(GPGSIds.achievement_practiced, 100f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }

        if (score >= 40)
        {
            Social.ReportProgress(GPGSIds.achievement_expert, 100f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }

        if (score >= 50)
        {
            Social.ReportProgress(GPGSIds.achievement_master, 100f, (bool success) =>
            {
                if (success)
                {
                    //Debug.Log("Get Achievement");
                }
                else
                {
                    //Debug.Log("Fail to get Achievement");
                }
            });
        }
    }


    // 점수를 증가시키는 메서드
    public void AddScore(int newScore) 
    {
        // 게임오버가 아니라면
        if (!isGameover)
        {
            // 점수를 증가
            score += newScore;
            scoreText.text = "Score : " + score;
        }
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead() 
    {
        isGameover = true;

        // 최고 점수 저장
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            if (SoundControl.bSoundOn) audioFanfare.Play();
        }
        bestScoreText.text = "Best score : " + bestScore; 
    }
}