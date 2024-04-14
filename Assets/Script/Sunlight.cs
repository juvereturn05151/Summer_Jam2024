using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sunlight : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _sunlightImage;

    [SerializeField]
    private float startingAlpha = 0.25f;

    private float _currentAlpha;

    public bool ActivateSunlight;

    private Light2D light;
    [SerializeField] private float normalSunlightIntensity = 0.1f;
    [SerializeField] private float sunlightFlashIntensity = 0.5f;

    private void Start()
    {
        SetAlpha(startingAlpha);
        
        light = GetComponentInChildren<Light2D>();
        light.intensity = normalSunlightIntensity;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SetAlpha(1.0f);
            light.intensity = sunlightFlashIntensity;
            ActivateSunlight = true;
        }
        else 
        {
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
            if (collision.tag == "Vampire")
            {
                Destroy(collision.gameObject);
            }
        }

    }
}
