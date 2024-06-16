using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameplayUIManager : MonoBehaviour
{
    private static GameplayUIManager _instance;
    private static string _name = "GameplayUIManager";

    // Public property to access the singleton instance
    public static GameplayUIManager Instance
    {
        get
        {
            // If the instance doesn't exist, try to find it in the scene
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameplayUIManager>();

                // If it still doesn't exist, create a new GameObject with the SingletonExample component
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(_name);
                    _instance = singletonObject.AddComponent<GameplayUIManager>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject gamePlayUI;
    [SerializeField] private Slider waterSlider;
    public Slider WaterSlider => waterSlider;
    [SerializeField] private ChangeFillImage _waterFillImage;
    public ChangeFillImage WaterFillImage => _waterFillImage;
    [SerializeField] private GameObject _gameEndUI;
    [SerializeField] private GameObject _winningUI;
    [SerializeField] private GameObject _losingUI;
    [SerializeField] private Slider _levelProgressSlider;

    [SerializeField] private MMF_Player scoreFeedback;

    [SerializeField] private GameObject waterSplashFX;
    [SerializeField] private GameEndPanelManager _gameEndPanel;

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
    [SerializeField] private PrepareStateManager _prepareStateManager;
    public PrepareStateManager PrepareStateManager => _prepareStateManager;
    [SerializeField] private GameObject _storyModeUI;
    [SerializeField] private GameObject _survivalModeUI;
    [SerializeField] private Slider _progressionSlider;

    public void Init(GameMode mode)
    {
        if (mode == GameMode.StoryMode)
        {
            _storyModeUI.SetActive(true);
        }
        else 
        {
            _survivalModeUI.SetActive(true);
            highScoreText.text = $"High score: {ScoreManager.HighScores[GameManager.Instance.CurrentStage]}";
        }
    }

    public void UpdateUI(GameMode mode) 
    {
        if (mode == GameMode.StoryMode)
        {
            _levelProgressSlider.value = (ScoreManager.Scores[GameManager.Instance.CurrentStage] / GameManager.Instance.Phase4Score); 
        }
        else if (mode == GameMode.SurvivalMode)
        {
            scoreText.text = "Score: " + ScoreManager.Scores[GameManager.Instance.CurrentStage];
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

        GameManager.Instance.CheckScore();

        scoreFeedback.PlayFeedbacks();
    }

    public void Restart() 
    {
        ScoreManager.Scores[GameManager.Instance.CurrentStage] = 0;
        SoundManager.Instance.PlayOneShot("SFX_Click");
        FadingUI.Instance.StartFadeIn();
        FadingUI.OnStopFading += LoadGameplay;
    }

    public void LoadGameplay()
    {
        SceneManager.LoadScene("Level" + (GameManager.Instance.CurrentStage + 1));
    }

    public void LoadNextGameplay()
    {
        if (GameManager.Instance.CurrentStage >= ScoreManager.numberOfStage) 
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
        FadingUI.OnStopFading += LoadMainScene;
    }

    public void OnGameOver(bool isWinning, GameMode mode) 
    {
        _gameEndPanel.ActivateEndGameUI(isWinning,mode);
        gamePlayUI.SetActive(false);
        waterSlider.gameObject.SetActive(false);

        if (mode == GameMode.StoryMode) 
        {
            _progressionSlider.value = (ScoreManager.Scores[GameManager.Instance.CurrentStage] / GameManager.Instance.Phase4Score);
        }
        else if (mode == GameMode.SurvivalMode)
        {
            _scoreText.text = $"Score: {ScoreManager.Scores[GameManager.Instance.CurrentStage]}";
            _highScoreText.text = $"HighScore: {ScoreManager.HighScores[GameManager.Instance.CurrentStage]}";
            _passingScoreText.text = $"You Need To Score At Least: {GameManager.Instance.Phase4Score}";
        }
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
