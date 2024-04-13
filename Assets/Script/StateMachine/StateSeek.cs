using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSeek : State
{
    public GameObject TargetPos;
    public StateSeek(AIAgent aiAgent, GameObject target) : base(aiAgent)
    {
        AIAgent = aiAgent;
        TargetPos = target;
    }
    public override void OnEnter() 
    {
    
    }
    public override void OnState() 
    {
        AIAgent.Seek(TargetPos);
    }
    public override void OnExit() 
    {
        
    }
}
