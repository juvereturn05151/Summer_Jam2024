using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static MainMenuPanel MainMenuPanel;
    public static GameplayPanel GameplayPanel;
    public static MenuPanel MenuPanel;

    [SerializeField] private MainMenuPanel mainMenuPanel;
    [SerializeField] private GameplayPanel gameplayPanel;
    [SerializeField] private MenuPanel menuPanel;

    private void Awake()
    {
        InitStaticPanel();
    }

    private void InitStaticPanel()
    {
        MainMenuPanel = mainMenuPanel;
        GameplayPanel = gameplayPanel;
        MenuPanel = menuPanel;
    }
}
