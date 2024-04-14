using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSeek : State
{
    public GameObject Target;
    public StateSeek(AIAgent aiAgent, GameObject target) : base(aiAgent)
    {
        AIAgent = aiAgent;
        Target = target;
    }
    public override void OnEnter() 
    {
    
    }

    public override void OnState() 
    {
        if (AIAgent != null && Target != null) 
        {
            AIAgent.Seek(Target);
        }
    }

    public override void OnExit() 
    {
        
    }
}
