using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Status
{
    Freeze,
    Liquid,
    Gas
}
public class Pond : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    #region -Sunlight-

    [Header("Sunlight Variables")]
    [SerializeField] private float iceMeltTimeBySun = 3f;
    
    public float IceMeltTimeBySun
    {
        get => iceMeltTimeBySun;
        set => iceMeltTimeBySun = value;
    }

    [SerializeField] private float pondTimeEvaporate = 5f;

    public float PondTimeEvaporate
    {
        get => pondTimeEvaporate;
        set => pondTimeEvaporate = value;
    }

    #endregion

    #region -Moonlight-

    [Header("Moonlight Variables")]
    [SerializeField] private float freezeTimeByMoon = 5f;

    public float FreezeTimeByMoon
    {
        get => freezeTimeByMoon;
        set => freezeTimeByMoon = value;
    }

    #endregion

    [Space]
    [SerializeField] private Status objectStat;

    public Status ObjectStat
    {
        get => objectStat;
        set => objectStat = value;
    }

    [SerializeField] private float pondFill;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckObjectStatus()
    {
        switch (objectStat)
        {
            case Status.Freeze:
                objectStat = Status.Liquid;
                break;
            case Status.Liquid:
                if(TimeManager.Instance._TimePhase == TimePhase.Night) 
                    objectStat = Status.Freeze;
                if (TimeManager.Instance._TimePhase == TimePhase.Morning)
                    objectStat = Status.Gas;    
                break;
        }
    }

    public void IceMeltToPond()
    {
        print("Ice change to pond");
        _spriteRenderer.color = Color.blue;
        CheckObjectStatus();
    }

    public void PondEvaporate()
    {
        print("water is run out!");
        Destroy(gameObject, 1f);
        CheckObjectStatus();
    }

    public void FreezePond()
    {
        print("Pond have freeze into ice...");
        _spriteRenderer.color = Color.white;
        CheckObjectStatus();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Human>() is Human human)
        {
            print("human drink water");
            PlayerBase.Instance.IncreaseWaterAmount(pondFill);
            Destroy(gameObject, human.DrinkWaterTimer);
        }
    }
}
