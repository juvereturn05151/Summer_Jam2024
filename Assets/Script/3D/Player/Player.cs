using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] Transform visual;
    [SerializeField] float moveSpeed;

    Rigidbody rb;
    [HideInInspector] public Vector2 moveInput;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void LateUpdate()
    {
        LockAtCam();
    }

    void HandleMovement()
    {
        Vector3 moveDir = Camera.main.transform.forward * moveInput.y;
        moveDir = moveDir + Camera.main.transform.right * moveInput.x;
        moveDir.Normalize();
        moveDir.y = 0;
        moveDir = moveDir * moveSpeed;

        rb.linearVelocity = moveDir;

    }

    void LockAtCam()
    {
        visual.LookAt(visual.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

}
