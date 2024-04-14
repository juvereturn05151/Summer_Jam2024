using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sunlight : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _sunlightImage;

    [SerializeField]
    private float _startingAlpha = 0.25f;

    private float _maximumAlpha = 1.0f;

    private float _currentAlpha;

    public bool ActivateSunlight;

    private Light2D light;
    [SerializeField] private float normalSunlightIntensity = 0.1f;
    [SerializeField] private float sunlightFlashIntensity = 0.5f;

    private void Start()
    {
        SetAlpha(_startingAlpha);
        SetAlpha(startingAlpha);
        
        light = GetComponentInChildren<Light2D>();
        light.intensity = normalSunlightIntensity;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _currentAlpha += Time.deltaTime;
            SetAlpha(_currentAlpha);

            if (_currentAlpha >= _maximumAlpha) 
            {
                ActivateSunlight = true;
            }
            SetAlpha(1.0f);
            light.intensity = sunlightFlashIntensity;
            ActivateSunlight = true;
        }
        else 
        {
            _currentAlpha = _startingAlpha;
            SetAlpha(_startingAlpha);
            SetAlpha(startingAlpha);
            light.intensity = normalSunlightIntensity;
            ActivateSunlight = false;
        }
    }

    private void SetAlpha(float newAlpha) 
    {
        var tempColor = _sunlightImage.color;
        tempColor.a = newAlpha;
        _sunlightImage.color = tempColor;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ActivateSunlight) 
        {
            if (collision.gameObject.GetComponent<Enemy>() is Enemy enemy)
            {
                enemy.Stun();
                enemy.DecreaseHealth();
            }
        }
    }
}
