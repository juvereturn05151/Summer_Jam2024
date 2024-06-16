using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.Stop("BGM_Gameplay");
        SoundManager.Instance.Play("BGM_Title");
        ScoreManager.InitScore();
    }

}
