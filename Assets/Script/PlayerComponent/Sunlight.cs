using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
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

    List<Pond> sightedPonds = new List<Pond>();

    [SerializeField] private GameObject hole;

    [SerializeField] private float createHoldTime;
    private float currentCreateHoleTime;

    [SerializeField] private CircleCollider2D circleCollider2D;

    [Header("Particle FX")]
    [SerializeField] private GameObject lightParticle;
    [SerializeField] private GameObject meltSnowFX;
    [SerializeField] private GameObject fireMeltFX;
    
    private GameObject lightFX_Temp;
    private bool isLighting;

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
                for (int i = 0; i < sightedEnemies.Count; i++)
                {
                    if (sightedEnemies[i] != null)
                    {
                        sightedEnemies[i].Stun();
                        sightedEnemies[i].DecreaseHealth(sunlightDamageToEnemy * Time.deltaTime);
                        currentCreateHoleTime = 0;
                    }
                }

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
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                var snowMelt = Instantiate(meltSnowFX, transform.position + Vector3.up, quaternion.identity);
                Destroy(snowMelt, 1f);
                timer = 0;
            }

            if (lightFX_Temp == null)
            {
                if (!isLighting)
                {
                    isLighting = true;
                    lightFX_Temp = Instantiate(lightParticle, transform.position, quaternion.identity, transform);   
                }

                // timer += Time.deltaTime;
                // if (timer >= 0.25f)
                // {
                //     var fireFX = Instantiate(fireMeltFX, transform.position, quaternion.identity);
                //     Destroy(fireFX, 0.7f);
                // }
            }

            for(int i = 0; i < sightedEnemies.Count; i++)
            {
                if (sightedEnemies[i] != null)
                {
                    sightedEnemies[i].Stun();
                    sightedEnemies[i].DecreaseHealth(sunlightDamageToEnemy * Time.deltaTime);
                    currentCreateHoleTime = 0;
                }
            }

            for (int i = 0; i < sightedPonds.Count; i++)
            {
                if (sightedPonds[i] != null)
                {
                    sightedPonds[i].PondTimeEvaporate -= Time.deltaTime;
                    currentCreateHoleTime = 0;
                }
            }

            currentCreateHoleTime += Time.deltaTime;
            if (currentCreateHoleTime >= createHoldTime) 
            {
                SoundManager.Instance.PlayOneShot("PondSFX");
                
                var fireFX = Instantiate(fireMeltFX, transform.position, quaternion.identity);
                Destroy(fireFX, 0.7f);
                
                var holeClone = Instantiate(hole, transform.position, transform.rotation);
                Pond pond = holeClone.GetComponent<Pond>();

                if (!sightedPonds.Contains(pond))
                {
                    sightedPonds.Add(pond);
                }
                currentCreateHoleTime = 0;

                var lightFX = Instantiate(lightParticle, holeClone.transform.position, quaternion.identity);
                Destroy(lightFX, 2f);

            }
        }
        else
        {
            isLighting = false;
            Destroy(lightFX_Temp);
        }

        if (sightedEnemies.Count == 0)
        {
            light.color = new Color(1.0f, 0.64f, 0.0f, 1);
        }
        else 
        {
            light.color = new Color(1.0f, 0.20f, 0.0f, 1);
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

                if (ActivateSunlight && sightedEnemies.Count > 1) 
                {
                    SoundManager.Instance.PlayOneShot("SFX_Expand");
                }
            }
        }

        if (collision.gameObject.GetComponent<Pond>() is Pond pond)
        {
            if (!sightedPonds.Contains(pond))
            {
                sightedPonds.Add(pond);
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

        if (collision.gameObject.GetComponent<Pond>() is Pond pond)
        {
            if (sightedPonds.Contains(pond))
            {
                sightedPonds.Remove(pond);
            }
        }
    }
}
