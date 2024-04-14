using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStun : State
{
    Enemy _enemy;
    float stunTime = 3.0f;
    float currentStunTime;

    public StateStun(AIAgent aiAgent, Enemy enemy) : base(aiAgent)
    {
        _enemy = enemy;
        AIAgent = aiAgent;
        currentStunTime = stunTime;
    }
    public override void OnEnter()
    {
        _enemy.StunEffect.SetActive(true);
    }
    public override void OnState()
    {
        currentStunTime -= Time.deltaTime;

        if (currentStunTime <= 0) 
        {
            AIAgent.ChangeState(new StateSeek(AIAgent, PlayerBase.Instance.gameObject));
        }
    }
    public override void OnExit()
    {
        _enemy.StunEffect.SetActive(false);
    }
}
