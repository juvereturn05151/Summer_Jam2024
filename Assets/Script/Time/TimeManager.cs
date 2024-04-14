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
    [Header("Time")]
    [SerializeField] private TextMeshProUGUI timeText;

    [Space]
    public float gameHourInSeconds;
    public float currentTimeInSeconds = 0;
    
    [Space]
    public float timeSpeed = 1.0f;
    public float time;
    public float hour;

    public TimePhase timePhase = TimePhase.Morning;

    [Space]
    [Header("Global Light")]
    public Light2D globalLight; // Reference to the directional light representing the sun
    
    [Space]
    [SerializeField] private float morningLight;
    [SerializeField] private float nightLight;

    // [SerializeField] private

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
        }
        
        UpdateTimeText();
        UpdateTimePhase();
        
        UpdateLightByTime();
    }

    private void UpdateTimeText()
    {
        int currentHour = Mathf.FloorToInt(currentTimeInSeconds / gameHourInSeconds) % 24;
        int currentMinute = Mathf.FloorToInt((currentTimeInSeconds % gameHourInSeconds) / (gameHourInSeconds / 60f));

        timeText.text = string.Format("{0:00}:{1:00}", currentHour, currentMinute);
    }

    void UpdateTimePhase()
    {
        int currentHour = Mathf.FloorToInt(currentTimeInSeconds / gameHourInSeconds) % 24;
        switch (currentHour)
        {
            case 4:
                timePhase = TimePhase.Morning;
                break;
            case 17:
                timePhase = TimePhase.Night;
                break;
        }
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
        switch (timePhase)
        {
            case TimePhase.Morning:
                if (globalLight.intensity >= nightLight && globalLight.intensity <= morningLight)
                {
                    globalLight.intensity += 0.025f * Time.deltaTime * timeSpeed;
                }
                break;
            case TimePhase.Night:
                if (globalLight.intensity >= nightLight)
                {
                    globalLight.intensity -= 0.025f * Time.deltaTime * timeSpeed;
                }
                else globalLight.intensity = nightLight;
                break;
        }
    }
}
