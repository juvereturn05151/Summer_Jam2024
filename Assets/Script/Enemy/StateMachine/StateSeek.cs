using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSeek : State
{
    public GameObject Target;
    private Vector3 targetPos;
    public StateSeek(AIAgent aiAgent, GameObject target) : base(aiAgent)
    {
        AIAgent = aiAgent;
        Target = target;
    }

    public StateSeek(AIAgent aiAgent, Vector3 targetPos) : base(aiAgent)
    {
        AIAgent = aiAgent;
        this.targetPos = targetPos;
    }

    public override void OnEnter() 
    {
    
    }

    public override void OnState() 
    {
        if (AIAgent != null) 
        {
            if (Target != null)
            {
                AIAgent.Seek(Target);
            }
            else
            {
                AIAgent.Seek(targetPos);
            }
        }


    }

    public override void OnExit() 
    {
        
    }
}
