using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFillImage : MonoBehaviour
{
    [SerializeField] private Image waterBarFill;
    
    [SerializeField] private Sprite normalWaterBar;

    [SerializeField] private Sprite hurtWaterBar;

    [SerializeField] private Sprite fillWaterBar;

    private bool isChangeValue;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        waterBarFill.sprite = normalWaterBar;
    }

    // Update is called once per frame
    void Update()
    {
        if (isChangeValue)
        {
            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                NormalSlider();
                timer = 0;
                isChangeValue = false;
            }
        }
    }

    public void HurtSlider()
    {
        isChangeValue = true;
        waterBarFill.sprite = hurtWaterBar;
    }

    public void NormalSlider()
    {
        waterBarFill.sprite = normalWaterBar;
    }

    public void FillWaterSlider()
    {
        isChangeValue = true;
        waterBarFill.sprite = fillWaterBar;
    }
}
