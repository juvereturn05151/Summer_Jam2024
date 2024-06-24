using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("===== Follow =====")]
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothSpeed = 0.125f;

    [Header("===== Corner =====")]
    [SerializeField] Transform corner1;
    [SerializeField] Transform corner2;

    private void LateUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (target != null)
        {
            Vector3 desiredPostion = target.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed);

            transform.position = smoothedPos;
            transform.position = Clamp(corner1.position, corner2.position);
        }
    }

    Vector3 Clamp(Vector3 minPos, Vector3 maxPos)
    {
        Vector3 pos = new Vector3(
            Mathf.Clamp(transform.position.x, minPos.x, maxPos.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, minPos.z, maxPos.z));

        return pos;
    }

}
