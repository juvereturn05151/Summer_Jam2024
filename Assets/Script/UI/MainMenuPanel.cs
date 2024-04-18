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
        SoundManager.Instance.Play("BGM_Title");
    }

    private void InitButtonAction()
    {
        startButton.onClick.AddListener(OnClickStart);
        tutorialButton.onClick.AddListener(OnClickTutorial);
        settingButton.onClick.AddListener(OnClickSetting);
        quitButton.onClick.AddListener(OnClickQuit);
    }

    //private void Update()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        Debug.Log($"OnClick");
    //        SoundManager.Instance.PlayOneShot("SFX_Click");
    //    }
    //}

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
        SceneManager.LoadScene("Tutorial");
        //Hide();
        //UIManager.GameplayPanel.Show();
    }

    private void OnClickTutorial()
    {
        //Call tutorial panal
        Debug.Log($"OnClickTutorial");
        SoundManager.Instance.PlayOneShot("SFX_Click");
        //UIManager.TutorialPanel.Show();
    }

    private void OnClickSetting()
    {
        //Call setting panal
        Debug.Log($"OnClickSetting");
        SoundManager.Instance.PlayOneShot("SFX_Click");
    }

    private void OnClickQuit()
    {
        Debug.Log($"OnClickQuit");
        SoundManager.Instance.PlayOneShot("SFX_Click");
        Application.Quit();
    }
    #endregion
}
