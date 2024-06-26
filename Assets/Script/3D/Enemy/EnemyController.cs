using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform visual;

    private void LateUpdate()
    {
        LockAtCam();
    }

    void LockAtCam()
    {
        visual.LookAt(visual.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

}
