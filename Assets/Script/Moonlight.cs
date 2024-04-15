using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Moonlight : MonoBehaviour
{
    [SerializeField] private bool isNight;
    public bool IsNight
    {
        get => isNight;
        set => isNight = value;
    }

    [SerializeField] private Light2D light;
    private CircleCollider2D col;

    [SerializeField] private float lightRadius = 0.5f;
    [SerializeField] private bool moonlightActivated;
    public bool MoonLightActivated
    {
        get => moonlightActivated;
        set => moonlightActivated = value;
    }

    [SerializeField] private float normalMoonlightIntensity = 0.1f;
    [SerializeField] private float moonlightFlashIntensity = 0.35f;

    private float timer;

    [Header("Moonlight Damage")]
    [SerializeField] private float timeTargetToDealDamage = 2f;
    [SerializeField] private float moonlightDamageToEnemy = 5f;
    

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        col.radius = lightRadius;
        light = GetComponentInChildren<Light2D>();
        light.intensity = normalMoonlightIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.Instance._TimePhase == TimePhase.Night)
        {
            isNight = true;
            light.gameObject.SetActive(true);
        }
        else
        {
            moonlightActivated = false;
            isNight = false;
            light.gameObject.SetActive(false);
        }
        FlashMoonlight();
    }

    void FlashMoonlight()
    {
        if (isNight)
        {
            if (Input.GetMouseButton(0))
            {
                if (light.intensity <= moonlightFlashIntensity)
                    light.intensity += Time.deltaTime;

                if (light.intensity >= moonlightFlashIntensity)
                    moonlightActivated = true;
            }
            else
            {
                light.intensity = normalMoonlightIntensity;
                moonlightActivated = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (moonlightActivated)
        {
            if (other.GetComponent<Enemy>() is Enemy enemy)
            {
                enemy.Stun();

                timer += Time.deltaTime;
                if (timer >= timeTargetToDealDamage)
                {
                    enemy.DecreaseHealth(moonlightDamageToEnemy);
                    timer = 0;
                }
            }

            if (other.GetComponent<Pond>() is Pond pond)
            {
                timer += Time.deltaTime;
                if (timer >= pond.FreezeTimeByMoon && pond.ObjectStat == Status.Liquid)
                {
                    pond.FreezePondToMelt();
                    timer = 0;
                }
            }
        }
    }
}
