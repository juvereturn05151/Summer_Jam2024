using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [Header("===== Resolution =====")]
    [SerializeField] Button nextResolutionButton;
    [SerializeField] Button previousResolutionButton;
    [SerializeField] TextMeshProUGUI curResolutionText;

    List<Resolution> filterResolution = new List<Resolution>();
    Resolution[] resolutions;

    RefreshRate refresRate;
    int curResolutionIndex = 0;

    [Header("===== Fullscreen =====")]
    [SerializeField] Button nextFullscreenButton;
    [SerializeField] Button previousFullscreenButton;
    [SerializeField] TextMeshProUGUI curFullscreenText;

    bool isFullscreen = true;

    private void Awake()
    {
        nextResolutionButton.onClick.AddListener(NextResolution);
        previousResolutionButton.onClick.AddListener(PrevResolution);

        nextFullscreenButton.onClick.AddListener(NextFullscreenButton);
        previousFullscreenButton.onClick.AddListener(NextFullscreenButton);

    }

    private void Start()
    {
        SetupScreen();
    }

    void SetupScreen()
    {
        refresRate = Screen.currentResolution.refreshRateRatio;
        resolutions = Screen.resolutions;

        GetResolution();

        curResolutionIndex = filterResolution.Count - 1;

        SetResolution(curResolutionIndex);

    }

    #region Resolution



    void GetResolution()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.numerator == refresRate.numerator)
            {
                filterResolution.Add(resolutions[i]);
            }
        }
    }

    void NextResolution()
    {
        if (curResolutionIndex >= filterResolution.Count - 1)
        {
            curResolutionIndex = 0;
        }
        else
        {
            curResolutionIndex++;
        }

        SetResolution(curResolutionIndex);
    }

    void PrevResolution()
    {
        if (curResolutionIndex <= 0)
        {
            curResolutionIndex = filterResolution.Count - 1;
        }
        else
        {
            curResolutionIndex--;
        }
        SetResolution(curResolutionIndex);
    }

    void SetResolution(int index)
    {
        Resolution resolution = filterResolution[index];
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
        UpdateResolutionText();
    }

    void UpdateResolutionText()
    {
        string text = $"{filterResolution[curResolutionIndex].width} x {filterResolution[curResolutionIndex].height} ( {filterResolution[curResolutionIndex].refreshRateRatio.ToString()}Hz )";
        curResolutionText.text = text;
    }

    #endregion


    #region Fullscreen

    void NextFullscreenButton()
    {
        isFullscreen = !isFullscreen;
        SetFullscreen();
    }

    void SetFullscreen()
    {
        SetResolution(curResolutionIndex);
        UpdateFullscreenText();
    }

    void UpdateFullscreenText()
    {
        string text = string.Empty;
        if (isFullscreen)
        {
            text = $"Fullscreen";
        }
        else
        {
            text = $"Window";
        }
        curFullscreenText.text = text;
    }

    #endregion
}
