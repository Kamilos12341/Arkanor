using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName = "Alden";

    [Header("Dialogue")]

    [SerializeField]
    private NPCDialogue defaultDialogue;

    [SerializeField]
    private NPCDialogue activeQuestDialogue;

    [SerializeField]
    private NPCDialogue completedQuestDialogue;

    /*
    
    {
      "Witaj, wędrowcze. Nieczęsto widuję tu nowych ludzi.",
        "Jeśli zmierzasz do Brzezin, zachowaj ostrożność.",
        "Wilki ostatnio podchodzą coraz bliżej drogi." 
    };
    */

    [SerializeField] private string questToStartID;

    private DialogueManager dialogueManager;

    private NPCAnimator npcAnimator;


    private void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        npcAnimator = GetComponentInChildren<NPCAnimator>();

    }

    public void Interact()
    {
        if (dialogueManager == null)
        {
            Debug.LogWarning("Nie znaleziono DialogueManager.");
            return;
        }

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null && npcAnimator != null)
        {
            Vector2 direction =
                playerObject.transform.position - transform.position;

            npcAnimator.FaceDirection(direction);
        }

        dialogueManager.StartDialogue(
            npcName,
            GetCurrentDialogue()
        );

        if (!string.IsNullOrEmpty(questToStartID))
        {
            QuestManager.Instance.StartQuest(questToStartID);
        }
    }

    private string[] GetCurrentDialogue()
    {
        if (string.IsNullOrEmpty(questToStartID))
        {
            return defaultDialogue.lines;
        }

        Quest quest = QuestManager.Instance.GetQuest(questToStartID);

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