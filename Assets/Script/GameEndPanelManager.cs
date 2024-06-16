using UnityEngine;

public class GameEndPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _winBG;

    [SerializeField]
    private GameObject _loseBG;

    [SerializeField]
    private EndAnimationController _winAnimation;

    [SerializeField]
    private EndAnimationController _loseAnimation;

    [SerializeField]
    private GameObject _appearOnAnimationEndPanel;

    [SerializeField]
    private GameObject _winPanel;

    [SerializeField]
    private GameObject _losePanel;

    [SerializeField]
    private GameObject[] _storyModePanels;

    [SerializeField]
    private GameObject[] _survivalModePanels;

    private bool _isWin;

    public void ActivateEndGameUI(bool isWin, GameMode mode) 
    {
        this.gameObject.SetActive(true);
        _winAnimation.SetGameEndPanel(this);
        _loseAnimation.SetGameEndPanel(this);
        ActivateModeSpecificUI(mode);

        _isWin = isWin;

        if (isWin)
        {
            _winBG.SetActive(true);
            _winAnimation.gameObject.SetActive(true);
        }
        else 
        {
            _loseBG.SetActive(true);
            _loseAnimation.gameObject.SetActive(true);
        }
    }

    public void OnAnimationEnd() 
    {
        _appearOnAnimationEndPanel.SetActive(true);

        if (_isWin)
        {
            _winPanel.gameObject.SetActive(true);
        }
        else
        {
            _losePanel.gameObject.SetActive(true);
        }
    }

    private void ActivateModeSpecificUI(GameMode mode) 
    {
        if (mode == GameMode.StoryMode)
        {
            foreach (GameObject gameObj in _storyModePanels)
            {
                gameObj.SetActive(true);
            }
        }
        else if (mode == GameMode.SurvivalMode) 
        {
            foreach (GameObject gameObj in _survivalModePanels)
            {
                gameObj.SetActive(true);
            }
        }
    }
}
