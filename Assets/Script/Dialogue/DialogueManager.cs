using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField]
    private Button nextButton;

    public float typingSpeed = 0.05f;

    private string[] dialogueLines;
    private int currentLineIndex = 0;
    private bool isTyping = false;

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
        nextButton.gameObject.SetActive(false);
    }
}