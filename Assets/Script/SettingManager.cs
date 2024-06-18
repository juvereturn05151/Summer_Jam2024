using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField] Button openSettingButton;
    [SerializeField] Button closeSettingButton;
    [SerializeField] GameObject settingPanel;

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

    [Header("===== Language =====")]
    [SerializeField] Button nextLanguageButton;
    [SerializeField] Button previousLanguageButton;
    [SerializeField] TextMeshProUGUI curLanguageText;

    [Header("===== Vsync =====")]
    [SerializeField] Button nextVsyncButton;
    [SerializeField] Button previousVsyncButton;
    [SerializeField] TextMeshProUGUI curVsyncText;

    bool isOpenVsync;

    private void Awake()
    {
        openSettingButton.onClick.AddListener(OpenSetting);
        closeSettingButton.onClick.AddListener(CloseSetting);

        nextResolutionButton.onClick.AddListener(NextResolution);
        previousResolutionButton.onClick.AddListener(PrevResolution);

        nextFullscreenButton.onClick.AddListener(NextFullscreenButton);
        previousFullscreenButton.onClick.AddListener(NextFullscreenButton);

        bgmUpButton.onClick.AddListener(bgmUpVolume);
        bgmDownButton.onClick.AddListener(bgmDownVolume);

        sfxUpButton.onClick.AddListener(sfxUpVolume);
        sfxDownButton.onClick.AddListener(sfxDownVolume);

        nextVsyncButton.onClick.AddListener(nextVsync);
        previousVsyncButton.onClick.AddListener(nextVsync);

    }

    private void Start()
    {
        SetupScreen();
        SetBGMVolume();
        SetSFXVolume();
        UpdateVsyncText();
    }

    #region Open Close Setting

    void OpenSetting()
    {
        settingPanel.SetActive(true);
    }

    void CloseSetting()
    {
        settingPanel.SetActive(false);
    }

    #endregion

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
        SoundManager.Instance.PlayOneShot("SFX_Click");
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
        SoundManager.Instance.PlayOneShot("SFX_Click");
    }

    void SetResolution(int index)
    {
        Resolution resolution = filterResolution[index];
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
        UpdateResolutionText();
        UpdateFullscreenText();
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
        SoundManager.Instance.PlayOneShot("SFX_Click");
        UpdateResolutionText();
    }

    void SetFullscreen()
    {
        Screen.fullScreen = !isFullscreen;
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
        SoundManager.Instance.PlayOneShot("SFX_Click");
    }

    void bgmDownVolume()
    {
        curBGMStep--;
        if (curBGMStep < minStep)
        {
            curBGMStep = minStep;
        }
        SetBGMVolume();
        SoundManager.Instance.PlayOneShot("SFX_Click");
    }

    void sfxUpVolume()
    {
        curSFXStep++;
        if (curSFXStep > maxStep)
        {
            curSFXStep = maxStep;
        }
        SetSFXVolume();
        SoundManager.Instance.PlayOneShot("SFX_Click");
    }

    void sfxDownVolume()
    {
        curSFXStep--;
        if (curSFXStep < minStep)
        {
            curSFXStep = minStep;
        }
        SetSFXVolume();
        SoundManager.Instance.PlayOneShot("SFX_Click");
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

    #region Language

    #endregion

    #region Vsync

    void nextVsync()
    {
        isOpenVsync = !isOpenVsync;

        if (isOpenVsync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        UpdateVsyncText();
    }

    void UpdateVsyncText()
    {
        if (isOpenVsync)
        {
            curVsyncText.text = "On";
        }
        else
        {
            curVsyncText.text = "Off";
        }
    }

    #endregion
}
