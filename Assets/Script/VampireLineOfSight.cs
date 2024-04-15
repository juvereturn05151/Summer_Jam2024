using System;
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

        if (TimeManager.Instance._TimePhase == TimePhase.Night)
        {
            if (collision.CompareTag(TagCollection.HumanTag))
            {
                _enemy.FoundHuman = true;
                _aiAgent.ChangeState(new StateSeek(_aiAgent, collision.gameObject));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(TagCollection.HumanTag))
        {
            _enemy.FoundHuman = false;
        }
    }
}
