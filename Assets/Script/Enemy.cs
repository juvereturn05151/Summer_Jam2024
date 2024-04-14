using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private AIAgent _aiAgent;

    [SerializeField]
    private GameObject _stunEffect;
    public GameObject StunEffect => _stunEffect;

    [SerializeField]
    private float _health;

    [SerializeField]
    private float _decreaseAmount;

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerBase>() is PlayerBase playerBase)
        {
            playerBase.DecreaseWaterAmount(_decreaseAmount);
        }
    }

    public void DecreaseHealth() 
    {
        _health -= Time.deltaTime;
        Debug.Log("_health" + _health);
        if (_health <= 0) 
        {
            Destroy(gameObject);
        }
    }

    public void Stun() 
    {
        _aiAgent.ChangeState(new StateStun(_aiAgent, this));
    }
}
