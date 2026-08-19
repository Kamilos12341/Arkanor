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

    private NPC currentNPC;

    private PlayerInputActions input;

    private void Update()
    {
        if (dialogueManager != null && dialogueManager.IsDialogueOpen)
        {
            playerMovement.CanMove = false;

            HideCurrentPrompt();

            if (input.Player.Interact.WasPressedThisFrame())
            {
                dialogueManager.NextLine();
            }

            return;
        }

        playerMovement.CanMove = true;

        FindInteractable();
        UpdateNPCDirection();

        if (input.Player.Interact.WasPressedThisFrame())
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
        NPC nearestNPC = null;

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

                nearestNPC =
                    hit.GetComponent<NPC>();
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
            currentNPC = nearestNPC;

            if (currentPrompt != null)
            {
                currentPrompt.Show();
            }
        }
    }

    private void UpdateNPCDirection()
    {
        if (currentNPC == null)
            return;

        NPCAnimator npcAnimator =
            currentNPC.GetComponentInChildren<NPCAnimator>();

        if (npcAnimator == null)
            return;

        npcAnimator.FacePlayer(
            transform,
            currentNPC.transform
        );
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

        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}