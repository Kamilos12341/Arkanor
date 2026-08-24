using UnityEngine;

namespace Arkanor.Player
{

    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator animator;
        private PlayerMovement movement;
        private Combat combat;

        private Vector2 lastDirection = Vector2.down;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            movement = GetComponent<PlayerMovement>();
            combat = GetComponent<Combat>();
        }

        private void Update()
        {
            Vector2 move = movement.Movement;

            bool isMoving = move != Vector2.zero;

            animator.SetBool("IsMoving", isMoving);

            animator.SetBool(
                "IsAttacking",
                combat != null && combat.IsAttacking
            );

            animator.SetFloat("MoveX", move.x);
            animator.SetFloat("MoveY", move.y);

            if (isMoving && (combat == null || !combat.IsAttacking))
            {
                lastDirection = move;

                animator.SetFloat("LastMoveX", lastDirection.x);
                animator.SetFloat("LastMoveY", lastDirection.y);
            }
        }
    }
}