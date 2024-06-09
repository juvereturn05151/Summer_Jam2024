using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThankYouForPlaying : MonoBehaviour
{
    public void LoadMainMenu() 
    {
        SceneManager.LoadScene("MainScene");
    }
}
