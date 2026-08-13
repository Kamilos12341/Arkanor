using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text dialogueText;

    private string[] dialogueLines;
    private int currentLine;

    public bool IsDialogueOpen { get; private set; }

    private void Start()
    {
        dialogueWindow.SetActive(false);
    }

    public void StartDialogue(string npcName, string[] lines)
    {
        if (lines == null || lines.Length == 0)
            return;

        dialogueLines = lines;
        currentLine = 0;

        npcNameText.text = npcName;
        dialogueText.text = dialogueLines[currentLine];

        dialogueWindow.SetActive(true);
        IsDialogueOpen = true;
    }

    public void NextLine()
    {
        if (!IsDialogueOpen)
            return;

        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            CloseDialogue();
            return;
        }

        dialogueText.text = dialogueLines[currentLine];
    }

    public void CloseDialogue()
    {
        dialogueWindow.SetActive(false);
        IsDialogueOpen = false;
    }
}