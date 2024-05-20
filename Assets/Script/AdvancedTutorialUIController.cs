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
}
