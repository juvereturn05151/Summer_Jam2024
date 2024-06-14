using UnityEngine;

public class EndAnimationController : MonoBehaviour
{
    private GameEndPanelManager _gameEndPanelManager;

    public void SetGameEndPanel(GameEndPanelManager gameEndPanelManager) 
    {
        _gameEndPanelManager = gameEndPanelManager;
    }

    public void OnAnimationEnd() 
    {
        _gameEndPanelManager.OnAnimationEnd();
    }
}
