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

    [SerializeField] 
    private TimePhase _timePhase = TimePhase.Morning;
    public TimePhase TimePhase => _timePhase;

    [Space]
    [Header("Global Light")]
    [SerializeField] private Light2D globalLight; // Reference to the directional light representing the sun
    
    [Space]
    [SerializeField]
    private float morningLight;
    public float MorningLight => morningLight;

    [SerializeField] 
    private float nightLight;
    public float NightLight => nightLight;

    [SerializeField] 
    private float lightTime = 0.025f;

    [SerializeField] 
    private float _timeSpeed = 2.0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else 
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetTimePhase(TimePhase.Morning);
    }

    private void Update()
    {   
        UpdateLightByTime();
    }

    public void SetTimePhase(TimePhase phase)
    {
        _timePhase = phase;
    }

    public void SetMorningLight(float morningLight) 
    {
        this.morningLight = morningLight;
    }

    private void UpdateLightByTime()
    {
        switch (_timePhase)
        {
            case TimePhase.Morning:

                if (globalLight.intensity >= nightLight && globalLight.intensity < morningLight)
                {
                    globalLight.intensity += lightTime * Time.deltaTime * _timeSpeed;
                }
                else 
                {
                    globalLight.intensity = morningLight;
                } 

                break;
            case TimePhase.Night:

                if (globalLight.intensity > nightLight)
                {
                    globalLight.intensity -= lightTime * Time.deltaTime * _timeSpeed;
                }
                else
                {
                    globalLight.intensity = nightLight;
                }
                    
                break;
        }
    }
}
