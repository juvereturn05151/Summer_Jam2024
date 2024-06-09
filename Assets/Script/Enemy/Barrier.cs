using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField]
    private float _barrierTime;

    private bool _isActive;

    private void Update()
    {
        if (_isActive) 
        {
            _barrierTime -= Time.deltaTime;

            if (_barrierTime <= 0) 
            {
                SetBarrierActive(false);
            }
        }
    }

    public void SetBarrierActive(bool active) 
    {
        gameObject.SetActive(active);
        _isActive = active;
    }
}
