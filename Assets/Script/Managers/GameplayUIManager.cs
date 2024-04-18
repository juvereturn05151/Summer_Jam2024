using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameplayUIManager : MonoBehaviour
{
    private static GameplayUIManager instance;

    // Public property to access the singleton instance
    public static GameplayUIManager Instance
    {
        get
        {
            // If the instance doesn't exist, try to find it in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<GameplayUIManager>();

                // If it still doesn't exist, create a new GameObject with the SingletonExample component
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("GameplayUIManager");
                    instance = singletonObject.AddComponent<GameplayUIManager>();
                }

                // Ensure the instance persists between scene changes
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // If this is the first instance, set it as the singleton instance
        instance = this;
    }

    [SerializeField] private TextMeshProUGUI scoreText;
    
    public GameObject gameOverUI;

    public Slider waterSlider;

    [SerializeField] private MMF_Player scoreFeedback;

    [SerializeField] private GameObject waterSplashFX;

    public GameObject WaterSplashFX
    {
        get => waterSplashFX;
        set => waterSplashFX = value;
    }

    public Transform waterSplashParent;

    // Update is called once per frame

    private void Start()
    {
        ScoreManager.highscore = PlayerPrefs.GetFloat("HighScore", 0);
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0)) 
        //{
        //    if (gameOverUI.activeSelf)
        //    {
        //        Restart();
        //    }
        //}

        scoreText.text = "Score: " + ScoreManager.score.ToString() + " HighScore: " + ScoreManager.highscore;
    }

    public void IncreaseScore(float value)
    {
        ScoreManager.score += value;
        if (ScoreManager.score >= ScoreManager.highscore) 
        {
            ScoreManager.highscore = ScoreManager.score;
            PlayerPrefs.SetFloat("HighScore", ScoreManager.highscore);
        }
        scoreFeedback.PlayFeedbacks();
    }

    public void Restart() 
    {
        //gameOverUI.SetActive(false);
        SoundManager.Instance.PlayOneShot("SFX_Click");
        FadingUI.Instance.StartFadeIn();
        FadingUI.Instance.OnStopFading.AddListener(LoadGameplay);
    }

    private void LoadGameplay()
    {
        SceneManager.LoadScene("QiqiRealGameplay");
    }

    public void Quit()
    {
        SoundManager.Instance.PlayOneShot("SFX_Click");
        FadingUI.Instance.StartFadeIn();
        FadingUI.Instance.OnStopFading.AddListener(LoadMainScene);
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
