using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        InitButtonAction();
    }

    private void Start()
    {
        SoundManager.Instance.Stop("BGM_Gameplay");
        SoundManager.Instance.Play("BGM_Title");
    }

    private void InitButtonAction()
    {
        startButton.onClick.AddListener(OnClickStart);
        tutorialButton.onClick.AddListener(OnClickTutorial);
        settingButton.onClick.AddListener(OnClickSetting);
        quitButton.onClick.AddListener(OnClickQuit);
    }

    #region Panel Action
    public void Show()
    {
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
    #endregion

    #region Button Action
    private void OnClickStart()
    {
        //Call start game
        Debug.Log($"OnClickStart");
        SoundManager.Instance.PlayOneShot("SFX_Click");
        FadingUI.Instance.StartFadeIn();
        FadingUI.Instance.OnStopFading.AddListener(LoadTutorialScene);
    }

    private void LoadTutorialScene() 
    {
        SceneManager.LoadScene("Tutorial");
    }

    private void OnClickTutorial()
    {
        //Call tutorial panal
        SoundManager.Instance.PlayOneShot("SFX_Click");
    }

    private void OnClickSetting()
    {
        //Call setting panal
        SoundManager.Instance.PlayOneShot("SFX_Click");
    }

    private void OnClickQuit()
    {
        SoundManager.Instance.PlayOneShot("SFX_Click");
        Application.Quit();
    }
    #endregion
}
