using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectionUIManager : MonoBehaviour
{
    private string _sceneIndex;

    public void Play(string sceneIndex)
    {
        FadingUI.Instance.StartFadeIn();
        _sceneIndex = sceneIndex;
        FadingUI.Instance.OnStopFading.AddListener(LoadScene);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("Level" + _sceneIndex);
    }
}
