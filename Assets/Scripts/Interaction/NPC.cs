using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName = "Alden";

    public void Interact()
    {
        Debug.Log($"Rozmawiasz z {npcName}.");
    }
}