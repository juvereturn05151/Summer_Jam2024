using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunlight : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _sunlightImage;

    [SerializeField]
    private float startingAlpha = 0.25f;

    private float _currentAlpha;

    public bool ActivateSunlight;

    private void Start()
    {
        SetAlpha(startingAlpha);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SetAlpha(1.0f);
            ActivateSunlight = true;
        }
        else 
        {
            SetAlpha(startingAlpha);
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
