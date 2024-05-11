using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static MainMenuPanel MainMenuPanel;
    public static GameplayPanel GameplayPanel;
    public static MenuPanel MenuPanel;
    public static TutorialPanel TutorialPanel;

    private static UIManager instance;

    [SerializeField] private MainMenuPanel mainMenuPanel;
    [SerializeField] private GameplayPanel gameplayPanel;
    [SerializeField] private MenuPanel menuPanel;
    [SerializeField] private TutorialPanel tutorialPanel;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        InitStaticPanel();
    }

    private void InitStaticPanel()
    {
        MainMenuPanel = mainMenuPanel;
        GameplayPanel = gameplayPanel;
        MenuPanel = menuPanel;
        TutorialPanel = tutorialPanel;
    }
}
