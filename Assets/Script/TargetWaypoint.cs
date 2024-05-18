using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWaypoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Human>() is Human human)
        {
            Destroy(gameObject);
        }
    }
}
