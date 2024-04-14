using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireLineOfSight : MonoBehaviour
{
    [SerializeField]
    private AIAgent _aiAgent;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(TagCollection.PlayerTag))
        {
            _aiAgent.ChangeState(new StateSeek(_aiAgent, collision.gameObject));
        }
    }
}
