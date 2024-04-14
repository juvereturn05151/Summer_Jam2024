using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    private AIAgent _aiAgent;
    
    [SerializeField] private float health;

    // Start is called before the first frame update
    void Start()
    {
        _aiAgent = GetComponent<AIAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecreaseHealth(float damage)
    {
        health -= damage;

        print($"health : {health}");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Moonlight>() is Moonlight moonlight)
        {
            if (moonlight.MoonLightActivated)
                _aiAgent.ChangeState(new StateSeek(_aiAgent, moonlight.gameObject));
            else 
                _aiAgent.ChangeState(new StateRetreat(_aiAgent));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Moonlight>())
        {
            _aiAgent.ChangeState(new StateRetreat(_aiAgent));
        }
    }
}
