using UnityEngine;
using Arkanor.Quests;
using Arkanor.Dialogue;

namespace Arkanor.NPC
{

    public class NPCController : MonoBehaviour, IInteractable
    {
        [SerializeField] private string npcName = "Alden";

        [Header("Dialogue")]

        [SerializeField]
        private NPCDialogue defaultDialogue;

        [SerializeField]
        private NPCDialogue activeQuestDialogue;

        [SerializeField]
        private NPCDialogue completedQuestDialogue;

        [SerializeField] private string questToStartID;

        private DialogueManager dialogueManager;
        private NPCAnimator npcAnimator;

        private void Start()
        {
            dialogueManager = DialogueManager.Instance;
            npcAnimator = GetComponentInChildren<NPCAnimator>();
        }

        public void Interact()
        {
            if (dialogueManager == null)
            {
                Debug.LogWarning(
                    $"NPC '{npcName}': Nie znaleziono DialogueManager."
                );

                return;
            }

            Quest quest = null;

            if (!string.IsNullOrEmpty(questToStartID))
            {
                if (QuestManager.Instance == null)
                {
                    Debug.LogWarning(
                        $"NPC '{npcName}': Nie znaleziono QuestManager."
                    );
                }
                else
                {
                    quest =
                        QuestManager.Instance.GetQuest(questToStartID);
                }
            }

            bool shouldCompleteQuest =
                quest != null &&
                quest.State == QuestState.Completed;

            dialogueManager.StartDialogue(
                npcName,
                GetCurrentDialogue(),
                shouldCompleteQuest
                    ? CompleteQuest
                    : StartQuestAfterDialogue
            );
        }

        private void StartQuestAfterDialogue()
        {
            if (string.IsNullOrEmpty(questToStartID))
                return;

            if (QuestManager.Instance == null)
            {
                Debug.LogWarning(
                    $"NPC '{npcName}': Nie znaleziono QuestManager. " +
                    $"Nie można rozpocząć questa '{questToStartID}'."
                );

                return;
            }

            QuestManager.Instance.StartQuest(questToStartID);
        }

        private void CompleteQuest()
        {
            if (string.IsNullOrEmpty(questToStartID))
                return;

            if (QuestManager.Instance == null)
            {
                Debug.LogWarning(
                    $"NPC '{npcName}': Nie znaleziono QuestManager. " +
                    $"Nie można ukończyć questa '{questToStartID}'."
                );

                return;
            }

            if (QuestManager.Instance.CompleteQuest(questToStartID))
            {
                Debug.Log($"Quest odebrany przez {npcName}.");
            }
        }

        private string[] GetCurrentDialogue()
        {
            if (string.IsNullOrEmpty(questToStartID))
            {
                return defaultDialogue.lines;
            }

            if (QuestManager.Instance == null)
            {
                Debug.LogWarning(
                    $"NPC '{npcName}': Nie znaleziono QuestManager."
                );

                return defaultDialogue.lines;
            }

            Quest quest =
                QuestManager.Instance.GetQuest(questToStartID);

            if (quest == null)
            {
                return defaultDialogue.lines;
            }

            switch (quest.State)
            {
                case QuestState.NotStarted:
                    return defaultDialogue.lines;

                case QuestState.Active:
                    return activeQuestDialogue.lines;

                case QuestState.Completed:
                    return completedQuestDialogue.lines;

                case QuestState.Rewarded:
                    return completedQuestDialogue.lines;

                default:
                    return defaultDialogue.lines;
            }
        }
    }
}