using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject fade;
    [Header("Button")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        InitButtonAction();
    }

    private void InitButtonAction()
    {
        restartButton.onClick.AddListener(OnClickRestart);
        tutorialButton.onClick.AddListener(OnClickTutorial);
        homeButton.onClick.AddListener(OnClickHome);
        closeButton.onClick.AddListener(OnClickClose);
    }

    #region Panel Action
    public void Show()
    {
        panel.SetActive(true);
        fade.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
        fade.SetActive(false);
    }
    #endregion

    #region Button Action
    private void OnClickRestart()
    {
        // Call restart game
        Debug.Log($"OnClickRestart");
        Hide();
    }

    private void OnClickTutorial()
    {
        // Call tutorial panal
        Debug.Log($"OnClickTutorial");
    }

    private void OnClickHome()
    {
        // Call main menu  panal
        Debug.Log($"OnClickHome");
        Hide();
        UIManager.GameplayPanel.Hide();
        UIManager.MainMenuPanel.Show();
    }

    private void OnClickClose()
    {
        // Call gamplay panal
        Debug.Log($"OnClickClose");
        Hide();
    }
    #endregion
}
