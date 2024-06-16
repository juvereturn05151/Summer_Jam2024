using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;
    [Header("Button")]
    [SerializeField] private Button menuButton;


    private void Awake()
    {
        InitButtonAction();
    }

    private void InitButtonAction()
    {
        menuButton.onClick.AddListener(OnClickMenu);
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
    private void OnClickMenu()
    {
        // Call menu panel
        // Call pause game
        Debug.Log($"OnClickMenu");
    }
    #endregion
}
