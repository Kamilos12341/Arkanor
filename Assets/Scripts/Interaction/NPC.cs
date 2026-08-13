using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName = "Alden";

    [TextArea(3, 5)]
    [SerializeField]
    private string[] dialogueLines =
    {
        "Witaj, wędrowcze. Nieczęsto widuję tu nowych ludzi.",
        "Jeśli zmierzasz do Brzezin, zachowaj ostrożność.",
        "Wilki ostatnio podchodzą coraz bliżej drogi."
    };

    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }

    public void Interact()
    {
        if (dialogueManager == null)
        {
            Debug.LogWarning("Nie znaleziono DialogueManager.");
            return;
        }

        dialogueManager.StartDialogue(
            npcName,
            dialogueLines
        );
    }


}