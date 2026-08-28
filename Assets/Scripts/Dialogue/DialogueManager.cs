using System;
using TMPro;
using UnityEngine;

namespace Arkanor.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {

        public static DialogueManager Instance { get; private set; }
        [SerializeField] private GameObject dialogueWindow;
        [SerializeField] private TMP_Text npcNameText;
        [SerializeField] private TMP_Text dialogueText;

        private string[] dialogueLines;
        private int currentLine;

        private Action onDialogueFinished;

        public bool IsDialogueOpen { get; private set; }

        public event Action DialogueOpened;
        public event Action DialogueClosed;

        private void Start()
        {
            dialogueWindow.SetActive(false);
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
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

            DialogueOpened?.Invoke();
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
            if (!IsDialogueOpen)
                return;

            dialogueWindow.SetActive(false);
            IsDialogueOpen = false;

            DialogueClosed?.Invoke();

            Action callback = onDialogueFinished;
            onDialogueFinished = null;

            callback?.Invoke();
        }
    }
}