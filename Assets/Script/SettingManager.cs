using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
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

    [Header("===== Audio Setting =====")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] int minStep = -8;
    [SerializeField] int maxStep = 2;
    [SerializeField] int volumePerStep = 10;

    [Header("- BGM")]
    [SerializeField] Button bgmUpButton;
    [SerializeField] Button bgmDownButton;
    [SerializeField] Image bgmFill;

    [Header("- SFX ")]
    [SerializeField] Button sfxUpButton;
    [SerializeField] Button sfxDownButton;
    [SerializeField] Image sfxFill;


    int curBGMStep = 0;
    int curSFXStep = 0;


    private void Awake()
    {
        nextResolutionButton.onClick.AddListener(NextResolution);
        previousResolutionButton.onClick.AddListener(PrevResolution);

        nextFullscreenButton.onClick.AddListener(NextFullscreenButton);
        previousFullscreenButton.onClick.AddListener(NextFullscreenButton);

        bgmUpButton.onClick.AddListener(bgmUpVolume);
        bgmDownButton.onClick.AddListener(bgmDownVolume);

        sfxUpButton.onClick.AddListener(sfxUpVolume);
        sfxDownButton.onClick.AddListener(sfxDownVolume);

    }

    private void Start()
    {
        SetupScreen();
        SetBGMVolume();
        SetSFXVolume();
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

    #region Audio Setting

    void bgmUpVolume()
    {
        curBGMStep++;
        if (curBGMStep > maxStep)
        {
            curBGMStep = maxStep;
        }
        SetBGMVolume();
    }

    void bgmDownVolume()
    {
        curBGMStep--;
        if (curBGMStep < minStep)
        {
            curBGMStep = minStep;
        }
        SetBGMVolume();
    }

    void sfxUpVolume()
    {
        curSFXStep++;
        if (curSFXStep > maxStep)
        {
            curSFXStep = maxStep;
        }
        SetSFXVolume();
    }

    void sfxDownVolume()
    {
        curSFXStep--;
        if (curSFXStep < minStep)
        {
            curSFXStep = minStep;
        }
        SetSFXVolume();
    }

    void SetBGMVolume()
    {
        int volume = curBGMStep * volumePerStep;
        mixer.SetFloat("MusicVolume", volume);
        UpdateBGMFill();
    }

    void SetSFXVolume()
    {
        int volume = curSFXStep * volumePerStep;
        mixer.SetFloat("SfxVolume", volume);
        UpdateSFXFill();
    }

    void UpdateSFXFill()
    {
        float volumeRange = Mathf.Abs(minStep) + Mathf.Abs(maxStep);
        float curVolume = curSFXStep + Mathf.Abs(minStep);

        float percent = curVolume / volumeRange;
        sfxFill.fillAmount = percent;
    }

    void UpdateBGMFill()
    {
        float volumeRange = Mathf.Abs(minStep) + Mathf.Abs(maxStep);
        float curVolume = curBGMStep + Mathf.Abs(minStep);

        float percent = curVolume / volumeRange;
        bgmFill.fillAmount = percent;
    }

    #endregion

}
