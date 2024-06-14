using UnityEngine;

public enum MoveDirection
{
    Left,
    Right
}

public class AIAgent : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 2f;
    public float MoveSpeed { get; private set; }

    public State currentState; // Current state of the AI agent

    private void Update()
    {
        // Update the current state
        if (currentState != null)
        {
            currentState.OnState();
        }
    }

    public void ChangeState(State newState)
    {
        // Exit the current state
        if (currentState != null)
        {
            currentState.OnExit();
        }

        // Enter the new state
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter();
        }
    }

    public virtual void SetOnSpawn(GameObject target)
    {
        ChangeState(new StateSeek(this, target));
    }

    public void Seek(GameObject TargetPos) 
    {
        // Calculate the direction to the target
        Vector3 direction = (TargetPos.transform.position - transform.position).normalized;

        // Move towards the target position
        transform.Translate(direction * _moveSpeed *Time.deltaTime * GameManager.Instance.GameTimeScale);

        // Check if the AI agent has reached the target position with some tolerance
        // Debug.Log(Vector3.Distance(transform.position, TargetPos.transform.position));
        if (Vector3.Distance(transform.position, TargetPos.transform.position) <= 0.1f)
        {
            // If reached, transition to another state (e.g., idle)
        }
    }

    public void Seek(Vector3 TargetPos)
    {
        // Calculate the direction to the target
        Vector3 direction = (TargetPos - transform.position).normalized;

        // Move towards the target position
        transform.Translate(direction * _moveSpeed * Time.deltaTime * GameManager.Instance.GameTimeScale);

        // Check if the AI agent has reached the target position with some tolerance
        Debug.Log(Vector3.Distance(transform.position, TargetPos));
        if (Vector3.Distance(transform.position, TargetPos) <= 0.1f)
        {
            // If reached, transition to another state (e.g., idle)
        }
    }
}
