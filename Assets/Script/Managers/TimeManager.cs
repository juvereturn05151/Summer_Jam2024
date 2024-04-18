using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public enum TimePhase
{
    Morning,
    Night
}

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Space]
    [SerializeField] private float gameHourInSeconds;
    [SerializeField] private float currentTimeInSeconds = 0;
    
    [Space]
    [SerializeField] private float timeSpeed = 1.0f;
    [SerializeField] private float time;
    [SerializeField] private float hour;

    [SerializeField] private int dayCount;

    [SerializeField] private TimePhase _timePhase = TimePhase.Morning;
    public TimePhase _TimePhase
    {
        get => _timePhase;
        set => _timePhase = value;
    }

    [Space]
    [Header("Global Light")]
    [SerializeField] private Light2D globalLight; // Reference to the directional light representing the sun
    
    [Space]
    [SerializeField] private float morningLight;
    public float MorningLight
    {
        get => morningLight;
        set => morningLight = value;
    }
    [SerializeField] private float nightLight;

    [SerializeField] private float lightTime = 0.025f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        SetTimePhase(TimePhase.Morning);
    }

    private void Update()
    {
        // Update current time based on time speed
        currentTimeInSeconds += Time.deltaTime * timeSpeed;
        time += Time.deltaTime * timeSpeed;
        hour = time / gameHourInSeconds;

        if (hour >= 24)
        {
            time = 0;
            hour = 0;
            dayCount++;
        }
        
        UpdateTimeText();
        
        UpdateLightByTime();
    }

    private void UpdateTimeText()
    {
        int currentHour = Mathf.FloorToInt(currentTimeInSeconds / gameHourInSeconds) % 24;
        int currentMinute = Mathf.FloorToInt((currentTimeInSeconds % gameHourInSeconds) / (gameHourInSeconds / 60f));
    }

    public void SetTimePhase(TimePhase phase)
    {
        switch (phase)
        {
            case TimePhase.Morning:
                currentTimeInSeconds = 4 * gameHourInSeconds;
                UpdateTimeText();
                break;
            case TimePhase.Night:
                currentTimeInSeconds = 17 * gameHourInSeconds;
                UpdateTimeText();
                break;
        }
    }

    void UpdateLightByTime()
    {
        switch (_timePhase)
        {
            case TimePhase.Morning:
                if (globalLight.intensity >= nightLight && globalLight.intensity < morningLight)
                {
                    globalLight.intensity += lightTime * Time.deltaTime * timeSpeed;
                }
                else globalLight.intensity = morningLight;
                break;
            case TimePhase.Night:
                if (globalLight.intensity > nightLight)
                {
                    globalLight.intensity -= lightTime * Time.deltaTime * timeSpeed;
                }
                else globalLight.intensity = nightLight;
                break;
        }
    }
}
