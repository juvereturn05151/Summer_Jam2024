using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialUIManager : MonoBehaviour
{
    public void Start()
    {
        SoundManager.Instance.Play("BGM_Title");
    }

    public void Play()
    {
        SceneManager.LoadScene("QiqiRealGameplay");
    }
}
