using System;
using TMPro;
using UnityEngine;

namespace Arkanor.Dialogue
{

    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueWindow;
        [SerializeField] private TMP_Text npcNameText;
        [SerializeField] private TMP_Text dialogueText;

        private string[] dialogueLines;
        private int currentLine;

        private Action onDialogueFinished;

        public bool IsDialogueOpen { get; private set; }

        private void Start()
        {
            dialogueWindow.SetActive(false);
        }

        public void StartDialogue(
            string npcName,
            string[] lines,
            Action onFinished = null)
        {
            if (lines == null || lines.Length == 0)
                return;

            dialogueLines = lines;
            currentLine = 0;

            onDialogueFinished = onFinished;

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

            Action callback = onDialogueFinished;
            onDialogueFinished = null;

            callback?.Invoke();
        }
    }
}