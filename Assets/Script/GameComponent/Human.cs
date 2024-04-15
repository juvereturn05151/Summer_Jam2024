using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] private HumanAIAgent _humanAIAgent;
    public HumanAIAgent HumanAIAgent => _humanAIAgent;

    [SerializeField] private float health;

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
                _humanAIAgent.ChangeState(new StateSeek(_humanAIAgent, moonlight.gameObject));
            else
                _humanAIAgent.ChangeState(new StateRetreat(_humanAIAgent));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Moonlight>())
        {
            _humanAIAgent.ChangeState(new StateRetreat(_humanAIAgent));
        }
    }
}
