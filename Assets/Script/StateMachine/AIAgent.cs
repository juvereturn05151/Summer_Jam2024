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

    public MoveDirection MoveDirection;

    public State currentState; // Current state of the AI agent



    private void Start()
    {

    }

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

    public void SetOnSpawn(MoveDirection moveDirection)
    {
        MoveDirection = moveDirection;

        if (moveDirection == MoveDirection.Left)
        {
            ChangeState(new StateMove(this, -_moveSpeed));
        }
        else
        {
            ChangeState(new StateMove(this, _moveSpeed));
        }
    }

    public void Seek(GameObject TargetPos) 
    {
        // Calculate the direction to the target
        Vector3 direction = (TargetPos.transform.position - transform.position).normalized;

        // Move towards the target position
        transform.Translate(direction * Time.deltaTime);

        // Check if the AI agent has reached the target position with some tolerance
        Debug.Log(Vector3.Distance(transform.position, TargetPos.transform.position));
        if (Vector3.Distance(transform.position, TargetPos.transform.position) <= 0.1f)
        {
            Debug.Log("des");
            // If reached, transition to another state (e.g., idle)
            //OnCatch(TargetPos);
        }
    }

   /* public void OnCatch(GameObject target) 
    {
        Destroy(target);
        Destroy(this.gameObject);
    }*/
}
