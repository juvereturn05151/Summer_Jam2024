using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStun : State
{
    private Enemy _enemy;
    private float _stunTime = 3.0f;
    private float _currentStunTime;

    public StateStun(AIAgent aiAgent, Enemy enemy) : base(aiAgent)
    {
        _enemy = enemy;
        AIAgent = aiAgent;
        _currentStunTime = _stunTime;
    }

    public override void OnEnter()
    {
        _enemy.StunEffect.SetActive(true);
        _enemy.SetIsStunning(true);
    }

    public override void OnState()
    {
        _currentStunTime -= Time.deltaTime;

        if (_currentStunTime <= 0)
        {
            _enemy.SetIsStunning(false);
            AIAgent.ChangeState(new StateSeek(AIAgent, Human.Instance.gameObject));
        }
        else
        {
            _enemy.SetIsStunning(true);
        } 
    }

    public override void OnExit()
    {
        _enemy.StunEffect.SetActive(false);
    }
}
