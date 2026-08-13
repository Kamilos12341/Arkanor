using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 1.2f;
    [SerializeField] private LayerMask interactionLayer;

    private IInteractable currentInteractable;
    private InteractionPrompt currentPrompt;

    private PlayerMovement playerMovement;
    private DialogueManager dialogueManager;

    private void Update()
    {
        if (dialogueManager != null && dialogueManager.IsDialogueOpen)
        {
            playerMovement.CanMove = false;

            HideCurrentPrompt();

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                dialogueManager.NextLine();
            }

            return;
        }

        playerMovement.CanMove = true;

        FindInteractable();

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    private void FindInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            interactionRadius,
            interactionLayer
        );

        IInteractable nearestInteractable = null;
        InteractionPrompt nearestPrompt = null;

        float nearestDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            IInteractable interactable =
                hit.GetComponent<IInteractable>();

            if (interactable == null)
                continue;

            float distance = Vector2.Distance(
                transform.position,
                hit.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestInteractable = interactable;
                nearestPrompt =
                    hit.GetComponent<InteractionPrompt>();
            }
        }

        if (nearestInteractable != currentInteractable)
        {
            if (currentPrompt != null)
            {
                currentPrompt.Hide();
            }

            currentInteractable = nearestInteractable;
            currentPrompt = nearestPrompt;

            if (currentPrompt != null)
            {
                currentPrompt.Show();
            }
        }
    }

    private void HideCurrentPrompt()
    {
        if (currentPrompt != null)
        {
            currentPrompt.Hide();
        }
    }

    private void Interact()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            interactionRadius
        );
    }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }
}