using UnityEngine;
using Arkanor.NPC;
using Arkanor.Dialogue;
using Arkanor.UI;

namespace Arkanor.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private LayerMask interactionLayer;

        private IInteractable currentInteractable;
        private InteractionPrompt currentPrompt;

        private PlayerMovement playerMovement;
        private PlayerInputHandler input;
        private DialogueManager dialogueManager;

        private NPCController currentNPC;

        private InteractionDetector interactionDetector;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            input = GetComponent<PlayerInputHandler>();
            dialogueManager = DialogueManager.Instance;
            interactionDetector = GetComponentInChildren<InteractionDetector>();
        }

        private void Start()
        {
            if (dialogueManager == null)
            {
                Debug.LogError(
                    "PlayerInteraction nie znalazł DialogueManager."
                );

                return;
            }

            dialogueManager.DialogueOpened += OnDialogueOpened;
            dialogueManager.DialogueClosed += OnDialogueClosed;
        }

        private void OnDestroy()
        {
            if (dialogueManager == null)
                return;

            dialogueManager.DialogueOpened -= OnDialogueOpened;
            dialogueManager.DialogueClosed -= OnDialogueClosed;
        }

        private void Update()
        {
            if (dialogueManager != null &&
                dialogueManager.IsDialogueOpen)
            {
                HideCurrentPrompt();

                if (input.Interact.WasPressedThisFrame())
                {
                    dialogueManager.NextLine();
                }

                return;
            }

            FindInteractable();
            UpdateNPCDirection();

            if (input.Interact.WasPressedThisFrame())
            {
                Interact();
            }
        }

        private void OnDialogueOpened()
        {
            playerMovement.CanMove = false;

            HideCurrentPrompt();
        }

        private void OnDialogueClosed()
        {
            playerMovement.CanMove = true;
        }

        private void FindInteractable()
        {
            if (interactionDetector == null)
                return;

            IInteractable nearestInteractable =
                interactionDetector.GetNearestInteractable();

            InteractionPrompt nearestPrompt = null;
            NPCController nearestNPC = null;

            if (nearestInteractable is MonoBehaviour interactableObject)
            {
                nearestPrompt =
                    interactableObject.GetComponent<InteractionPrompt>();

                nearestNPC =
                    interactableObject.GetComponent<NPCController>();
            }

            if (nearestInteractable == currentInteractable)
                return;

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
    }
}