using System.Collections.Generic;
using UnityEngine;

namespace Arkanor.Player
{
    public class InteractionDetector : MonoBehaviour
    {
        private readonly List<IInteractable> interactables = new();

        public IReadOnlyList<IInteractable> Interactables =>
            interactables;

        private void OnTriggerEnter2D(Collider2D other)
        {
            IInteractable interactable =
                other.GetComponent<IInteractable>();

            if (interactable == null)
                return;

            if (interactables.Contains(interactable))
                return;

            interactables.Add(interactable);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            IInteractable interactable =
                other.GetComponent<IInteractable>();

            if (interactable == null)
                return;

            interactables.Remove(interactable);
        }

        public IInteractable GetNearestInteractable()
        {
            IInteractable nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (IInteractable interactable in interactables)
            {
                if (interactable == null)
                    continue;

                MonoBehaviour interactableObject =
                    interactable as MonoBehaviour;

                if (interactableObject == null)
                    continue;

                float distance = Vector2.Distance(
                    transform.position,
                    interactableObject.transform.position
                );

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = interactable;
                }
            }

            return nearest;
        }
    }
}