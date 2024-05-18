using System.Collections.Generic;
using UnityEngine;

public class AdvancedTutorialManager : MonoBehaviour
{
    [SerializeField]
    private DialogueManager dialogueManager;

    [SerializeField]
    private GameObject _backGround;

    [SerializeField]
    private GameObject _sunImage;

    [SerializeField]
    private GameObject _textBox;

    [SerializeField]
    private GameObject _nextButton;

    [SerializeField]
    private List<TutorialStep> _tutorialList = new List<TutorialStep>();

    public TutorialStep CurrentTutorial { get; private set; }

    private bool _isOperating;
    private int _currentTutorialIndex;

    private void Start()
    {
        GameManager.Instance.State = GameManager.GameState.Stop;

        if (_tutorialList != null && _tutorialList.Count > 0) 
        {
            CurrentTutorial = _tutorialList[0];
            dialogueManager.StartDialogue(CurrentTutorial.DialogueLines, CurrentTutorial.ObjectiveDialogue);
            dialogueManager._onDialogueEnd.RemoveAllListeners();
            dialogueManager._onDialogueEnd.AddListener(CurrentTutorial.StartOperating);
            dialogueManager._onDialogueEnd.AddListener(OnDialogueEnd);
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
                    _nextButton.SetActive(true);
                    NextTutorial();
                }
            }
        }
    }

    public void OnDialogueEnd() 
    {
        _isOperating = true;
        _backGround.SetActive(false);
        _nextButton.SetActive(false);
        GameManager.Instance.State = GameManager.GameState.StartGame;
    }

    private void NextTutorial() 
    {
        _currentTutorialIndex++;

        if (_currentTutorialIndex >= _tutorialList.Count)
            return;

        if (_tutorialList != null && _tutorialList.Count > 0)
        {
            CurrentTutorial = _tutorialList[_currentTutorialIndex];
            dialogueManager.StartDialogue(CurrentTutorial.DialogueLines, CurrentTutorial.ObjectiveDialogue);
            dialogueManager._onDialogueEnd.RemoveAllListeners();
            dialogueManager._onDialogueEnd.AddListener(CurrentTutorial.StartOperating);
            dialogueManager._onDialogueEnd.AddListener(OnDialogueEnd);
        }
    }
}
