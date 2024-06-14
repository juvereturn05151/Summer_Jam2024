using UnityEngine;

public class ThreeTwoOneGoAnimController : MonoBehaviour
{
    private PrepareStateManager _prepareStateManager;

    public void SetPrepareStateManager(PrepareStateManager prepareStateManager) 
    {
        _prepareStateManager = prepareStateManager;
    }

    public void OnAnimationEnd()
    {
        _prepareStateManager.After321Go();
    }
}
