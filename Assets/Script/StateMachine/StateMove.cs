using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMove : State
{
    private float moveSpeed; // Speed at which to move to the right

    public StateMove(AIAgent aiAgent, float moveSpeed) : base(aiAgent)
    {
        this.moveSpeed = moveSpeed;
    }

    public override void OnEnter()
    {
        // Actions to perform when entering the move right state
    }

    public override void OnState()
    {
        // Move the AI agent to the right
        if (AIAgent != null) 
        {
            AIAgent.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    public override void OnExit()
    {
        // Actions to perform when exiting the move right state
    }
}
