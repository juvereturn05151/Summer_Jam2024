using UnityEngine;

public class ReaperEnemy : MonoBehaviour
{
    private void Start()
    {
        TimeManager.Instance.SetMorningLight(0.0f);
    }

    private void OnDestroy()
    {
        TimeManager.Instance.SetMorningLight(0.5f);
    }
}
