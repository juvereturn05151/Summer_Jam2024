using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField]
    private Button nextButton;

    public float typingSpeed = 0.05f;
    public UnityEvent _onDialogueEnd = new UnityEvent();

    private string[] dialogueLines;
    private int currentLineIndex = 0;
    private bool isTyping = false;
    private string objectiveDialogue;

    void Start()
    {
        nextButton.onClick.AddListener(DisplayNextLine);
        nextButton.gameObject.SetActive(false);
    }

    public void StartDialogue(string[] lines)
    {
        dialogueLines = lines;
        currentLineIndex = 0;
        nextButton.gameObject.SetActive(true);
        StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
    }

    public void StartDialogue(string[] lines, string objective)
    {
        dialogueLines = lines;
        currentLineIndex = 0;
        nextButton.gameObject.SetActive(true);
        StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
        objectiveDialogue = objective;
    }

    public void DisplayNextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = dialogueLines[currentLineIndex];
            isTyping = false;
        }
        else
        {
            currentLineIndex++;
            if (currentLineIndex < dialogueLines.Length)
            {
                StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    private void EndDialogue()
    {
        dialogueText.text = "";
        dialogueText.text = objectiveDialogue;
        nextButton.gameObject.SetActive(false);
        _onDialogueEnd?.Invoke();
    }
}