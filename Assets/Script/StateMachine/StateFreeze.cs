using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFreeze : State
{
    private Enemy _enemy;

    public StateFreeze(AIAgent aiAgent, Enemy enemy) : base(aiAgent)
    {
        _enemy = enemy;
        AIAgent = aiAgent;
    }

    public override void OnEnter()
    {
        _enemy.FreezeEffect.SetActive(true);
    }

    public override void OnState()
    {
        if (TimeManager.Instance._TimePhase == TimePhase.Night)
        {
            if (!_enemy.FoundHuman)
                _enemy.IsFreeze = true;
            else
                _enemy.IsFreeze = false;
        }
        else 
            _enemy.IsFreeze = false;
    }

    public override void OnExit()
    {
        _enemy.FreezeEffect.SetActive(false);
    }
}
