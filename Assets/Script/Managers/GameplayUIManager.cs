using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    public GameObject gameOverUI;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + ScoreManager.score.ToString();
    }

    public void Restart() 
    {
        ScoreManager.score = 0;
        SceneManager.LoadScene("QiqiRealGameplay");
    }
}
