using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pond : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private float meltTimeBySun = 3f;

    public float MeltTimeBySun
    {
        get => meltTimeBySun;
        set => meltTimeBySun = value;
    }

    [SerializeField] private float meltTimeByMoon = 5f;

    public float MeltTimeByMoon
    {
        get => meltTimeByMoon;
        set => meltTimeByMoon = value;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MeltToPond()
    {
        _spriteRenderer.color = Color.white;
    }
}
