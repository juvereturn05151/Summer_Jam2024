using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] Transform visual;

    [HideInInspector] public Vector2 moveInput;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        LockAtCam();
    }

    void LockAtCam()
    {
        visual.LookAt(visual.position + Camera.main.transform.rotation * Vector3.forward , Camera.main.transform.rotation * Vector3.up);
    }

}
