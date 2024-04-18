using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using Unity.Mathematics;

public class Sunlight : MonoBehaviour
{


    [SerializeField] private bool isMorning;
    public bool IsMorning
    {
        get => isMorning;
        set => isMorning = value;
    }
    
    [Header("Sunlight (Sprite Renderer)")]
    [SerializeField] private SpriteRenderer _sunlightImage;
    [SerializeField] private float _startingAlpha = 0.25f;
    private float _maximumAlpha = 1.0f;
    private float _currentAlpha;

    [Space]
    public bool ActivateSunlight;

    [Header("Sunlight (Light)")]
    [SerializeField] private Light2D light;
    private CircleCollider2D col;
    
    [SerializeField] private float sunlightRadius = 0.5f;
    
    [SerializeField] private float normalSunlightIntensity = 0.1f;
    [SerializeField] private float sunlightFlashIntensity = 0.5f;

    private float timer;
    [SerializeField] private float timeTargetToDamage;

    [Header("Sunlight Damage")]
    [SerializeField] private float sunlightDamageToEnemy = 10f;

    List<Enemy> sightedEnemies = new List<Enemy>();

    [SerializeField] private GameObject hole;

    [SerializeField] private float createHoldTime;
    private float currentCreateHoleTime;

    [SerializeField] private CircleCollider2D circleCollider2D;

    [Header("Particle FX")]
    [SerializeField] private GameObject lightParticle;

    private void Start()
    {
        SetAlpha(_startingAlpha);

        light = GetComponentInChildren<Light2D>();
        light.intensity = normalSunlightIntensity;

        col = GetComponent<CircleCollider2D>();
        col.radius = sunlightRadius;
    }

    private void Update()
    {
        if(GameManager.Instance.State == GameManager.GameState.EndGame)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);

        if (TimeManager.Instance._TimePhase == TimePhase.Morning)
        {
            isMorning = true;
            light.gameObject.SetActive(true);
        }
        else
        {
            ActivateSunlight = false;
            isMorning = false;
            light.gameObject.SetActive(false);
        }
            
        if (isMorning)
        {

            if (GameManager.Instance.State == GameManager.GameState.EndGame)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                SoundManager.Instance.Play("SFX_Burn");
            }

            if (Input.GetMouseButtonUp(0))
            {
                SoundManager.Instance.Stop("SFX_Burn");
            }

            if (Input.GetMouseButton(0))
            {

                _currentAlpha += Time.deltaTime * 2.0f;
                SetAlpha(_currentAlpha);

                if (light.intensity < sunlightFlashIntensity)
                    light.intensity += Time.deltaTime;

                if (sightedEnemies.Count > 1)
                {
                    light.pointLightOuterRadius = sightedEnemies.Count;
                    light.pointLightInnerRadius = sightedEnemies.Count * 0.75f;
                    circleCollider2D.radius = sightedEnemies.Count;
                }
                else 
                {
                    light.pointLightOuterRadius = 1f;
                    light.pointLightInnerRadius = 0.75f;
                    circleCollider2D.radius = 1f;
                }

                

                if (_currentAlpha >= _maximumAlpha || light.intensity >= sunlightFlashIntensity)
                {
                    if (Human.Instance.AmountWater > 0) 
                    {
                        ActivateSunlight = true;
                        Human.Instance.DecreaseWaterAmount(Time.deltaTime * 1.5f);
                    } 
                }
            }
            else
            {
                _currentAlpha = _startingAlpha;
                SetAlpha(_startingAlpha);

                light.intensity = normalSunlightIntensity;
                light.pointLightOuterRadius = 1f;
                light.pointLightInnerRadius = 0.75f;
                circleCollider2D.radius = 1f;
                ActivateSunlight = false;
            }
        }

        if (ActivateSunlight)
        {
            for(int i = 0; i < sightedEnemies.Count; i++)
            {
                if (sightedEnemies[i] != null)
                {
                    sightedEnemies[i].Stun();
                    sightedEnemies[i].DecreaseHealth(sunlightDamageToEnemy * Time.deltaTime);
                    currentCreateHoleTime = 0;
                }
            }

            currentCreateHoleTime += Time.deltaTime;
            if (currentCreateHoleTime >= createHoldTime) 
            {
                var holeClone = Instantiate(hole, transform.position, transform.rotation);
                currentCreateHoleTime = 0;

                var lightFX = Instantiate(lightParticle, holeClone.transform.position, quaternion.identity);
                Destroy(lightFX, 2f);
            }
        }


    }

    private void SetAlpha(float newAlpha) 
    {
        var tempColor = _sunlightImage.color;
        tempColor.a = newAlpha;
        _sunlightImage.color = tempColor;
    }

    void AdjustRadius(float radius)
    {
        col.radius = radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() is Enemy enemy)
        {
            if (!sightedEnemies.Contains(enemy))
            {
                sightedEnemies.Add(enemy);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() is Enemy enemy)
        {
            if (sightedEnemies.Contains(enemy))
            {
                sightedEnemies.Remove(enemy);
            }
        }
    }

   /* private void OnTriggerStay2D(Collider2D collision)
    {
        if (ActivateSunlight) 
        {
            if (collision.gameObject.GetComponent<Enemy>() is Enemy enemy)
            {
                enemy.Stun();
                enemy.DecreaseHealth(sunlightDamageToEnemy * Time.deltaTime);
            }

            if (collision.GetComponent<Human>() is Human human)
            {
                timer += Time.deltaTime;
                if (timer >= timeTargetToDamage)
                {
                    human.DecreaseHealth(sunlightDamageToHuman);
                    timer = 0;
                }
            }
        }
    }*/
}
