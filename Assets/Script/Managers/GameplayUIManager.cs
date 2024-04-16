using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + ScoreManager.score.ToString();
    }
}
