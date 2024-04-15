using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireLineOfSight : MonoBehaviour
{
    [SerializeField] private AIAgent _aiAgent;

    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(TagCollection.PlayerTag))
        {
            if (_enemy.IsStunning)
                _aiAgent.ChangeState(new StateSeek(_aiAgent, collision.gameObject));
        }
    }
}
