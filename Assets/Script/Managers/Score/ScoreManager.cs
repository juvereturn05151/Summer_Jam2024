using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager 
{
    private static bool isInit;
    public static readonly int numberOfStage = 5;
    private static int[] scores;
    public static int[] Scores
    {
        get
        {
            if (!isInit)
            {
                InitScore();
            }

            return scores;
        }
    }
    private static int[] highScores;
    public static int[] HighScores 
    {
        get 
        {
            if (!isInit) 
            {
                InitScore();
            }

            return highScores;
        }
    } 

    public static void InitScore() 
    {
        if (isInit)
            return;

        isInit = true;
        scores =  new int[numberOfStage];
        highScores = new int[numberOfStage];

        for(int i = 0; i < scores.Length; i++) 
        {
            scores[i] = 0;
        }

        for (int i = 0; i < highScores.Length; i++)
        {
            highScores[i] = PlayerPrefs.GetInt("highscore" + i.ToString(), 0);
        }
    }

    public static void SaveHighScore(int highScore, int scoreNumber) 
    {
        highScores[scoreNumber] = highScore;
        PlayerPrefs.SetInt("highscore" + scoreNumber.ToString(), highScore);
        PlayerPrefs.Save();
    }

    public static void AddScore(int value, int scoreNumber) 
    {
        scores[scoreNumber] += value;
    }
}
