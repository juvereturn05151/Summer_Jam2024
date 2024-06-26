using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] Transform visual;
    [SerializeField] float moveSpeed;

    Rigidbody rb;
    Animator anim;
    SpriteRenderer spriteRenderer;
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Vector2 mousePos;
    [SerializeField] LayerMask groundMask;
    [SerializeField] GameObject cone;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = visual.GetComponent<Animator>();
        spriteRenderer = visual.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        HandleMovement();
        MoveCone();
    }

    private void LateUpdate()
    {
        //LockAtCam();
    }

    void HandleMovement()
    {
        Vector3 moveDir = Camera.main.transform.forward * moveInput.y;
        moveDir = moveDir + Camera.main.transform.right * moveInput.x;
        moveDir.Normalize();
        moveDir.y = 0;
        moveDir = moveDir * moveSpeed;

        rb.linearVelocity = moveDir;
        MoveAnimationHandle();
    }

    void MoveAnimationHandle()
    {
        if (moveInput != Vector2.zero) anim.SetBool("isWalk", true);
        else anim.SetBool("isWalk", false);

        if (moveInput.x > 0) spriteRenderer.flipX = true;
        else if (moveInput.x < 0) spriteRenderer.flipX = false;
    }

    void LockAtCam()
    {
        visual.LookAt(visual.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    void MoveCone()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000, groundMask))
        {
            cone.transform.position = hit.point;
        }
    }

}
