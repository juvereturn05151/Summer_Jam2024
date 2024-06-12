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
    [SerializeField] private TextMeshProUGUI highScoreText;

    public GameObject gamePlayUI;

    public Slider waterSlider;

    [SerializeField] private ChangeFillImage _waterFillImage;
    public ChangeFillImage WaterFillImage => _waterFillImage;
    [SerializeField] private GameObject _gameEndUI;
    [SerializeField] private GameObject _winningUI;
    [SerializeField] private GameObject _losingUI;

    [SerializeField] private MMF_Player scoreFeedback;

    [SerializeField] private GameObject waterSplashFX;

    public GameObject WaterSplashFX
    {
        get => waterSplashFX;
        set => waterSplashFX = value;
    }

    public Transform waterSplashParent;

    [Header("GameOver Panel")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private TextMeshProUGUI _passingScoreText;
    // Update is called once per frame

    private void Start()
    {
        highScoreText.text = $"High score: {ScoreManager.HighScores[GameManager.Instance.CurrentStage]}";
    }

    private void Update()
    {
        scoreText.text = "Score: " + ScoreManager.Scores[GameManager.Instance.CurrentStage];

        if (GameManager.Instance.State == GameManager.GameState.EndGame)
        {
            gamePlayUI.SetActive(false);
            waterSlider.gameObject.SetActive(false);
            _scoreText.text = $"Score: {ScoreManager.Scores[GameManager.Instance.CurrentStage]}";
            _highScoreText.text = $"HighScore: {ScoreManager.HighScores[GameManager.Instance.CurrentStage]}";
            _passingScoreText.text = $"You Need To Score At Least: {GameManager.Instance.Phase4Score}";
        }
    }

    public void IncreaseScore(int value)
    {
        if (GameManager.Instance.CurrentStage > ScoreManager.Scores.Length) 
        {
            return;
        }

        ScoreManager.AddScore(value, GameManager.Instance.CurrentStage);

        if (ScoreManager.Scores[GameManager.Instance.CurrentStage] >= ScoreManager.HighScores[GameManager.Instance.CurrentStage]) 
        {
            ScoreManager.HighScores[GameManager.Instance.CurrentStage] = ScoreManager.Scores[GameManager.Instance.CurrentStage];
            ScoreManager.SaveHighScore(ScoreManager.HighScores[GameManager.Instance.CurrentStage], GameManager.Instance.CurrentStage);
        }

        scoreFeedback.PlayFeedbacks();
    }

    public void Restart() 
    {
        ScoreManager.Scores[GameManager.Instance.CurrentStage] = 0;
        SoundManager.Instance.PlayOneShot("SFX_Click");
        FadingUI.Instance.StartFadeIn();
        FadingUI.Instance.OnStopFading.AddListener(LoadGameplay);
    }

    public void LoadGameplay()
    {
        SceneManager.LoadScene("Level" + (GameManager.Instance.CurrentStage + 1));
    }

    public void LoadNextGameplay()
    {
        if (GameManager.Instance.CurrentStage <= ScoreManager.numberOfStage) 
        {
            SceneManager.LoadScene("ThankYouForPlaying");
            return;
        }

        SceneManager.LoadScene("Level" + (GameManager.Instance.CurrentStage + 1 + 1));
    }

    public void Quit()
    {
        SoundManager.Instance.PlayOneShot("SFX_Click");
        FadingUI.Instance.StartFadeIn();
        FadingUI.Instance.OnStopFading.AddListener(LoadMainScene);
    }

    public void OnGameOver(bool isWinning) 
    {
        if (_gameEndUI != null) 
        {
            _gameEndUI.SetActive(true);
        }

        if (isWinning)
        {
            if (_winningUI != null) 
            {
                _winningUI.SetActive(true);
            }
        }
        else
        {
            if (_losingUI != null) 
            {
                _losingUI.SetActive(true);
            }
        }
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
