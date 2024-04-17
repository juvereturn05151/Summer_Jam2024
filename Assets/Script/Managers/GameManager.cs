using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => instance;
    private static GameManager instance;

    public enum GameState
    {
        StartGame,
        EndGame
    }

    private GameState state;

    public GameState State 
    {
        get => state;
        set => state = value;
    }

    [SerializeField]
    private GameObject spawner2;

    [SerializeField]
    private GameObject spawner3;

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
        if (ScoreManager.score >= 300)
        {
            if (spawner3)
                spawner3.gameObject.SetActive(true);
        }
        else if(ScoreManager.score >= 150)
        {
            if (spawner2)
                spawner2.gameObject.SetActive(true);
        }

    }

    public void OnStartGame()
    {
        state = GameState.StartGame;
        ScoreManager.score = 0;
    }

    public void OnEndGame()
    {
        state = GameState.EndGame;
    }
}
