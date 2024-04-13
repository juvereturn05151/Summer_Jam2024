using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStun : State
{
    float stunTime = 3.0f;
    float currentStunTime;

    public StateStun(AIAgent aiAgent) : base(aiAgent)
    {
        AIAgent = aiAgent;
        currentStunTime = stunTime;
    }
    public override void OnEnter()
    {
        //AIAgent.GetComponent<PrototypeEnemy>()._stunEffect.SetActive(true);
    }
    public override void OnState()
    {
        /*AIAgent.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        currentStunTime -= Time.deltaTime;
        if (currentStunTime <= 0) 
        {
            AIAgent.CurrentState = new StateSeek(AIAgent);
        }*/
    }
    public override void OnExit()
    {
        //AIAgent.GetComponent<PrototypeEnemy>()._stunEffect.SetActive(false);
    }
}
