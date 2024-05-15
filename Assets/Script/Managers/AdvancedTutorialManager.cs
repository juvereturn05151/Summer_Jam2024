using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTutorialManager : MonoBehaviour
{
    [SerializeField]
    private DialogueManager dialogueManager;

    void Start()
    {
        string[] dialogueLines = {
            "Hello there!",
            "Welcome to our game.",
            "Enjoy your adventure!"
        };

        dialogueManager.StartDialogue(dialogueLines);
    }
}
