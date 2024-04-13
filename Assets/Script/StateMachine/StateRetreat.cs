using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRetreat : State
{
    public StateRetreat(AIAgent aiAgent) : base(aiAgent)
    {
        AIAgent = aiAgent;
    }
    public override void OnEnter()
    {

    }
    public override void OnState()
    {
        //AIAgent.Retreat();
    }
    public override void OnExit()
    {

    }
}
