using UnityEngine;

public class PrepareStateManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _chooseYourWarpPointUI;

    [SerializeField]
    private ThreeTwoOneGoAnimController _go321;

    private void Start()
    {
        _go321.SetPrepareStateManager(this);
    }

    public void OnStartPrepare() 
    {
        if (_chooseYourWarpPointUI != null) 
        {
            _chooseYourWarpPointUI.SetActive(true);
        }
    }

    public void OnPlayerSpawn() 
    {
        if (_chooseYourWarpPointUI != null)
        {
            _chooseYourWarpPointUI.SetActive(false);
            _go321.gameObject.SetActive(true);
        }
    }

    public void After321Go() 
    {
        _go321.gameObject.SetActive(false);
        GameManager.Instance.State = GameManager.GameState.StartGame;
    }
}
