using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperEnemy : MonoBehaviour
{
    private void Start()
    {
        TimeManager.Instance.MorningLight = 0.0f;
    }

    private void OnDestroy()
    {
        TimeManager.Instance.MorningLight = 0.5f;
    }
}
