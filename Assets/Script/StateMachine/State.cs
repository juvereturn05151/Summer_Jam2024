using UnityEngine;

public abstract class State
{
    protected AIAgent AIAgent; // Reference to the AI agent

    public State(AIAgent aiAgent)
    {
        this.AIAgent = aiAgent;
    }

    public virtual void OnEnter()
    {
        // Actions to perform when entering this state
    }

    public virtual void OnState()
    {
        // Actions to perform while in this state
    }

    public virtual void OnExit()
    {
        // Actions to perform when exiting this state
    }
}
