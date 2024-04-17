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

    void Update()
    {
        //if (Input.GetMouseButtonDown(0)) 
        //{
        //    if (gameOverUI.activeSelf)
        //    {
        //        Restart();
        //    }
        //}

        scoreText.text = "Score: " + ScoreManager.score.ToString();
    }

    public void IncreaseScore(float value)
    {
        ScoreManager.score += value;
        scoreFeedback.PlayFeedbacks();
    }

    public void Restart() 
    {
        //gameOverUI.SetActive(false);
        SceneManager.LoadScene("QiqiRealGameplay");
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainScene");
    }
}
