/* filename GameManager.cs
 * author   Tonnattan Chankasemporn
 * email:   juvereturn@gmail.com
 * 
 * Brief Description: 
 * This Class Contains All The Scene Names
 * /*/

using UnityEngine; /*Monobehaviour*/

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => instance;
    private static GameManager instance;

    public enum GameState
    {
        StartGame,
        EndGame,
        Stop
    }

    private GameState state;

    public GameState State 
    {
        get => state;
        set => state = value;
    }

    #region Tutorial Only

    [SerializeField]
    private bool _isTutorial;
    public bool IsTutorial => _isTutorial;

    public int BigPondDrank = 0;

    #endregion

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

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        OnStartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.Scores[GameManager.instance.CurrentStage] >= _phase4Score)
        {
            if (spawner5)
                spawner5.gameObject.SetActive(true);
        }
        else
        if(ScoreManager.Scores[GameManager.instance.CurrentStage] >= _phase3Score)
        {
            if (spawner4)
                spawner4.gameObject.SetActive(true);
        }
        else if (ScoreManager.Scores[GameManager.instance.CurrentStage] >= _phase2Score)
        {
            if (spawner3)
                spawner3.gameObject.SetActive(true);
        }
        else if(ScoreManager.Scores[GameManager.instance.CurrentStage] >= _phase1Score)
        {
            if (spawner2)
                spawner2.gameObject.SetActive(true);
        }

    }

    public void OnStartGame()
    {
        SoundManager.Instance.Stop("BGM_Title");
        SoundManager.Instance.Play("BGM_Gameplay");

        if (!IsTutorial) 
        {
            state = GameState.StartGame;
        }
    }

    public void OnEndGame()
    {
        state = GameState.EndGame;
    }

    public void DestroyOnWinning() 
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
