using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdvancedTutorialManager : MonoBehaviour
{
    public static AdvancedTutorialManager Instance => instance;
    private static AdvancedTutorialManager instance;

    [SerializeField]
    private DialogueManager dialogueManager;

    [SerializeField]
    private GameObject _backGround;

    [SerializeField]
    private GameObject _tutorialDisplayBackGround;

    [SerializeField]
    private GameObject _sunImage;

    [SerializeField]
    private GameObject _textBox;

    [SerializeField]
    private GameObject _nextButton;

    [SerializeField]
    private Button _skipButton;

    [SerializeField]
    private List<TutorialStep> _tutorialList = new List<TutorialStep>();

    [SerializeField]
    private AdvancedTutorialUIController _advancedTutorialUIController;

    public TutorialStep CurrentTutorial { get; private set; }

    private bool _isOperating;
    private int _currentTutorialIndex;

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
    }

    private void Start()
    {
        _skipButton.onClick.AddListener(LoadGameScene);

        GameManager.Instance.State = GameManager.GameState.Stop;

        if (_tutorialList != null && _tutorialList.Count > 0) 
        {
            CurrentTutorial = _tutorialList[0];
            dialogueManager.StartDialogue(CurrentTutorial.DialogueLines, CurrentTutorial.ObjectiveDialogue, CurrentTutorial.WhatToDoDialogue);
            dialogueManager._onDialogueEnd.RemoveAllListeners();
            dialogueManager._onSecondLineAppear.RemoveAllListeners();
            dialogueManager._onLastLineAppear.RemoveAllListeners();
            dialogueManager._onDialogueEnd.AddListener(CurrentTutorial.StartOperating);
            dialogueManager._onDialogueEnd.AddListener(OnDialogueEnd);
            dialogueManager._onSecondLineAppear.AddListener(OnSecondDialogue);
            dialogueManager._onLastLineAppear.AddListener(OnLastDialogue);
        }
    }

    private void Update()
    {
        if (CurrentTutorial != null) 
        {
            if (_isOperating) 
            {
                CurrentTutorial.TutorialAttribute.CheckingObjective();

                if (CurrentTutorial.TutorialAttribute.Clear) 
                {
                    _isOperating = false;
                    GameManager.Instance.State = GameManager.GameState.Stop;
                    _backGround.SetActive(true);
                    _nextButton.SetActive(true);
                    NextTutorial();
                }
            }
        }
    }

    public void OnSecondDialogue() 
    {
        if (CurrentTutorial.ShowOnSecondDialogue) 
        {
            _advancedTutorialUIController.AppearOnSecondDialogue[_currentTutorialIndex].SetActive(true);
        }
    }

    public void OnLastDialogue()
    {
        if (CurrentTutorial.ShowOnLastDialogue) 
        {
            _advancedTutorialUIController.AppearOnLastDialogue[_currentTutorialIndex].SetActive(true);
            _advancedTutorialUIController.AppearOnSecondDialogue[_currentTutorialIndex].SetActive(false);
        }
    }

    public void OnDialogueEnd() 
    {
        _isOperating = true;
        _backGround.SetActive(false);
        _nextButton.SetActive(false);
        _tutorialDisplayBackGround.SetActive(true); 
        _advancedTutorialUIController.AppearOnLastDialogue[_currentTutorialIndex].SetActive(false);
        _advancedTutorialUIController.AppearOnSecondDialogue[_currentTutorialIndex].SetActive(false);
        _advancedTutorialUIController.AdvancedTutorialUI[_currentTutorialIndex].SetActive(true);
        GameManager.Instance.State = GameManager.GameState.StartGame;
        SoundManager.Instance.Play("BGM_Gameplay");
    }

    private void NextTutorial() 
    {
        _advancedTutorialUIController.AdvancedTutorialUI[_currentTutorialIndex].SetActive(false);
        _advancedTutorialUIController.AppearOnSecondDialogue[_currentTutorialIndex].SetActive(false);
        _advancedTutorialUIController.AppearOnLastDialogue[_currentTutorialIndex].SetActive(false);
        _tutorialDisplayBackGround.SetActive(false);
        _currentTutorialIndex++;

        if (_currentTutorialIndex >= _tutorialList.Count) 
        {
            LoadGameScene();
            return;
        }

        if (_tutorialList != null && _tutorialList.Count > 0)
        {
            CurrentTutorial = _tutorialList[_currentTutorialIndex];

            if (CurrentTutorial.ShowTutorialGuideOnStart) 
            {
                _advancedTutorialUIController.AdvancedTutorialUI[_currentTutorialIndex].SetActive(true);
                _tutorialDisplayBackGround.SetActive(CurrentTutorial.ShowTutorialGuideBackground);
            }

            dialogueManager.StartDialogue(CurrentTutorial.DialogueLines, CurrentTutorial.ObjectiveDialogue, CurrentTutorial.WhatToDoDialogue);
            dialogueManager._onDialogueEnd.RemoveAllListeners();
            dialogueManager._onDialogueEnd.AddListener(CurrentTutorial.StartOperating);
            dialogueManager._onDialogueEnd.AddListener(OnDialogueEnd);
        }
    }

    private void LoadGameScene() 
    {
        FadingUI.Instance.StartFadeIn();
        FadingUI.Instance.OnStopFading.AddListener(LoadScene);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("QiqiRealGameplay");
    }
}
