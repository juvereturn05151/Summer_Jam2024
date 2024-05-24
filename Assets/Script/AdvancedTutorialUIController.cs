using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTutorialUIController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _advancedTutorialUI = new List<GameObject>();
    public List<GameObject> AdvancedTutorialUI => _advancedTutorialUI;

    [SerializeField]
    private List<GameObject> _appearOnSecondDialogue = new List<GameObject>();
    public List<GameObject> AppearOnSecondDialogue => _appearOnSecondDialogue;

    [SerializeField]
    private List<GameObject> _appearOnLastDialogue = new List<GameObject>();
    public List<GameObject> AppearOnLastDialogue => _appearOnLastDialogue;

    public void OnTutorialEnd(int _currentTutorialIndex) 
    {
        AdvancedTutorialUI[_currentTutorialIndex].SetActive(false);
        AppearOnSecondDialogue[_currentTutorialIndex].SetActive(false);
        AppearOnLastDialogue[_currentTutorialIndex].SetActive(false);
    }

    public void OnDialogueEnd(int _currentTutorialIndex) 
    {
        AppearOnLastDialogue[_currentTutorialIndex].SetActive(false);
        AppearOnSecondDialogue[_currentTutorialIndex].SetActive(false);
        AdvancedTutorialUI[_currentTutorialIndex].SetActive(true);
    }
}
