using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;
    
    private Vector3 _pos;

    // Update is called once per frame
    void Update()
    {
        _pos = Input.mousePosition;
        _pos.z = _speed;
        transform.position = Camera.main.ScreenToWorldPoint(_pos);
    }
}
