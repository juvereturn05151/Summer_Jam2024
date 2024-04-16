using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AIAgent _aiAgent;

    [Header("Enemy Stats")]
    [SerializeField] private GameObject _stunEffect;
    public GameObject StunEffect => _stunEffect;

    [SerializeField] private GameObject freezeEffect;
    public GameObject FreezeEffect => freezeEffect;

    [SerializeField] private float _health;

    [SerializeField] private float _decreaseAmount;

    [SerializeField] private bool isStunning;

    public bool IsStunning
    {
        get => isStunning;
        set => isStunning = value;
    }

    [Space]
    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private float destroyDropItemTime = 10f;

    [Space]
    [SerializeField] private bool isFreeze;

    public bool IsFreeze
    {
        get => isFreeze;
        set => isFreeze = value;
    }

    [SerializeField] private bool foundHuman;

    public bool FoundHuman
    {
        get => foundHuman;
        set => foundHuman = value;
    }

    private void Update()
    {
        if (TimeManager.Instance._TimePhase == TimePhase.Night)
            Freeze();
        else
        {
            if(isStunning) 
                _aiAgent.ChangeState(new StateSeek(_aiAgent, PlayerBase.Instance.gameObject));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerBase>() is PlayerBase playerBase)
        {
            playerBase.DecreaseWaterAmount(_decreaseAmount);
        }

        if (collision.gameObject.GetComponent<Human>() is Human Human)
        {
            PlayerBase.Instance.DecreaseWaterAmount(_decreaseAmount);
        }
    }

    public void DecreaseHealth(float damage)
    {
        // _health -= Time.deltaTime;
        _health -= damage;
        
        Debug.Log("_health" + _health);
        if (_health <= 0) 
        {
            Destroy(gameObject);
            var water = Instantiate(dropItemPrefab, transform.position, quaternion.identity);
            // Destroy(water, destroyDropItemTime);
        }
    }

    public void Stun() 
    {
        _aiAgent.ChangeState(new StateStun(_aiAgent, this));
    }

    public void Freeze()
    {
        if(!foundHuman)
            _aiAgent.ChangeState(new StateFreeze(_aiAgent, this));
    }
}
