/* filename GameManager.cs
 * author   Tonnattan Chankasemporn
 * email:   juvereturn@gmail.com
 * 
 * Brief Description: 
 * Manages GameState Involving Preparation
 * /*/

using UnityEngine; /*Monobehaviour*/

public enum GameState
{
    PreparingState,
    PlayingState,
    EndState,
    Stop
}

public enum GameMode
{
    TutorialMode,
    StoryMode,
    SurvivalMode,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    public GameState State { get; private set; }

    [SerializeField]
    private GameMode _mode;
    public GameMode Mode => _mode;

    [SerializeField]
    private int _currentStage;
    public int CurrentStage => _currentStage;

    [SerializeField]
    private GameObject spawner2;

    [SerializeField]
    private GameObject spawner3;

    [SerializeField]
    private GameObject spawner4;

    [SerializeField]
    private GameObject spawner5;

    [SerializeField]
    private float _phase1Score = 150;

    [SerializeField]
    private float _phase2Score = 300;

    [SerializeField]
    private float _phase3Score = 1000;

    [SerializeField]
    private float _phase4Score = 1800;
    public float Phase4Score => _phase4Score;

    public delegate void WhenGameEnd(bool isWin, GameMode mode);
    public static WhenGameEnd OnGameEnd;

    public int BigPondDrank { get; private set; } = 0;
    public float GameTimeScale { get; private set; } = 1;

    private GameplayUIManager _gameplayUIManager;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        InitOnStart();
    }

    // Update is called once per frame
    private void Update()
    {
        if (ScoreManager.Scores[CurrentStage] >= _phase4Score)
        {
            if (spawner5)
                spawner5.gameObject.SetActive(true);
        }
        else
        if(ScoreManager.Scores[CurrentStage] >= _phase3Score)
        {
            if (spawner4)
                spawner4.gameObject.SetActive(true);
        }
        else if (ScoreManager.Scores[CurrentStage] >= _phase2Score)
        {
            if (spawner3)
                spawner3.gameObject.SetActive(true);
        }
        else if(ScoreManager.Scores[CurrentStage] >= _phase1Score)
        {
            if (spawner2)
                spawner2.gameObject.SetActive(true);
        }

        _gameplayUIManager.UpdateUI(_mode);
    }

    public void SetGameTimeScale(float timeScale) 
    {
        GameTimeScale = timeScale;
    }

    public void IncreaseBigPondDrank() 
    {
        BigPondDrank++;
    }

    public void CheckScore() 
    {
        if (_mode == GameMode.StoryMode) 
        {
            if (ScoreManager.Scores[CurrentStage] > Phase4Score) 
            {
                OnGameEnd(true, _mode);
            }
        }
    }

    public void SetGameState(GameState newState) 
    {
        State = newState;
    }

    /* Brief
    If it's not the tutorial mode, we always start from the preparing state
    */

    private void InitOnStart() 
    {
        OnGameEnd += OnGameOver;

        _gameplayUIManager = GameplayUIManager.Instance;

        if (_gameplayUIManager != null) 
        {
            _gameplayUIManager.Init(_mode);

            if (Mode == GameMode.StoryMode || Mode == GameMode.SurvivalMode)
            {
                State = GameState.PreparingState;
                _gameplayUIManager.PrepareStateManager.OnStartPrepare();
            }
        }

        SoundManager.Instance.InitOnGameBegin();
    }

    private void OnGameOver(bool isWin, GameMode mode) 
    {
        State = GameState.EndState;
        DestroyAtEnd();
        SetGameTimeScale(0.0f);
    }

    private void DestroyAtEnd()
    {
        if (spawner2)
            spawner2.gameObject.SetActive(false);

        if (spawner3)
            spawner3.gameObject.SetActive(false);

        if (spawner4)
            spawner4.gameObject.SetActive(false);

        if (spawner5)
            spawner5.gameObject.SetActive(false);

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }
}
