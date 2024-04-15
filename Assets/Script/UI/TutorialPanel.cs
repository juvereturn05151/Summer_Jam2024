using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TutorialInfo
{
    public string title = "Title";
    public string detail = "Detail";
    public Image image;
}

[System.Serializable]
public class TutorialInfoGroup
{
    public List<TutorialInfo> tutorialInfos;
}

public class TutorialPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject fade;
    [Header("Button")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button closeButton;
    [Header("TutorialInfo")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Transform tutorialGroup;
    [SerializeField] private List<TutorialInfoGroup> infos;

    private int page = 0;
    private int maxPage = 0;

    private void Awake()
    {
        InitButtonAction();
    }

    private void InitButtonAction()
    {
        leftButton.onClick.AddListener(OnClickLeft);
        rightButton.onClick.AddListener(OnClickRight);
        closeButton.onClick.AddListener(OnClickClose);
    }

    private void InitPanel()
    {
        page = 0;
        maxPage = infos.Count;
        SetTutorialInfos(page);
        SetButtonActive(leftButton, !IsFirstPage());
        SetButtonActive(rightButton, !IsLastPage());
    }

    private void SetButtonActive(Button button, bool value)
    {
        button.gameObject.SetActive(value);
    }

    private void SetTutorialInfos(int page)
    {
        if (page > infos.Count - 1)
        {
            Debug.LogError($"Invalid tutorial infos");
            return;
        }

        TutorialInfoGroup tutorials = infos[page];

        for (int i = 0; i < tutorialGroup.childCount; i++)
        {
            GameObject tutorial = tutorialGroup.GetChild(i).gameObject;

            if (i < tutorials.tutorialInfos.Count)
            {
                tutorial.SetActive(true);
                TutorialInfoSlot slot;

                if (tutorial.TryGetComponent(out slot))
                {
                    slot.SetTutorialInfo(tutorials.tutorialInfos[i]);
                }
                else
                {
                    slot = tutorial.gameObject.AddComponent<TutorialInfoSlot>();
                    slot.SetTutorialInfo(tutorials.tutorialInfos[i]);
                }
            }
            else
            {
                tutorial.SetActive(false);
            }
        }
    }

    private void SetTitle(int page)
    {
        titleText.text = $"TUTORIAL #{page + 1}";
    }

    #region Page Checker
    private bool IsFirstPage()
    {
        return page == 0;
    }

    private bool IsLastPage()
    {
        if(maxPage == 0)
        {
            return true;
        }
        else
        {
            return page == maxPage - 1;
        }
    }
    #endregion

    #region Panel Action
    public void Show()
    {
        panel.SetActive(true);
        fade.SetActive(true);

        InitPanel();
    }

    public void Hide()
    {
        panel.SetActive(false);
        fade.SetActive(false);
    }
    #endregion

    #region Button Action
    private void OnClickLeft()
    {
        // Call previous tutorial
        Debug.Log($"OnClickLeft");
        if (IsFirstPage()) return;

        page--;
        SetTitle(page);
        SetTutorialInfos(page);
        SetButtonActive(leftButton, !IsFirstPage());
        SetButtonActive(rightButton, !IsLastPage());
    }

    private void OnClickRight()
    {
        // Call next tutorial
        Debug.Log($"OnClickRight");
        if (IsLastPage()) return;

        page++;
        SetTitle(page);
        SetTutorialInfos(page);
        SetButtonActive(leftButton, !IsFirstPage());
        SetButtonActive(rightButton, !IsLastPage());
    }

    private void OnClickClose()
    {
        // Close tutorial panal
        Debug.Log($"OnClickClose");
        Hide();
    }
    #endregion
}
