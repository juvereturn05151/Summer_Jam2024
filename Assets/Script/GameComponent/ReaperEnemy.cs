using UnityEngine;

public class ReaperEnemy : MonoBehaviour
{
    private TimeManager _timeManager;

    private void Start()
    {
        _timeManager = TimeManager.Instance;

        if (_timeManager != null) 
        {
            _timeManager.SetTimePhase(TimePhase.Night);
        }
    }

    private void OnDestroy()
    {
        if (_timeManager != null)
        {
            _timeManager.SetTimePhase(TimePhase.Morning);
        }
    }
}
