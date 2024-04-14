using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] private float sunlightDamageToHuman = 1;

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
            if (Input.GetMouseButton(0))
            {
                _currentAlpha += Time.deltaTime;
                SetAlpha(_currentAlpha);

                if (light.intensity < sunlightFlashIntensity)
                    light.intensity += Time.deltaTime;

                if (_currentAlpha >= _maximumAlpha || light.intensity >= sunlightFlashIntensity)
                {
                    ActivateSunlight = true;
                }
            }
            else
            {
                _currentAlpha = _startingAlpha;
                SetAlpha(_startingAlpha);

                light.intensity = normalSunlightIntensity;
                ActivateSunlight = false;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ActivateSunlight) 
        {
            if (collision.gameObject.GetComponent<Enemy>() is Enemy enemy)
            {
                enemy.Stun();
                
                timer += Time.deltaTime;
                if (timer >= timeTargetToDamage)
                {
                    enemy.DecreaseHealth(sunlightDamageToEnemy);
                    timer = 0;
                }
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

            if (collision.GetComponent<Pond>() is Pond pond)
            {
                timer += Time.deltaTime;
                if (timer >= pond.MeltTimeBySun)
                    pond.MeltToPond();
            }
        }
    }
}
