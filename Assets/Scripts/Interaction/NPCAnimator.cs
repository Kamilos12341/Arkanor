using UnityEngine;

public class NPCAnimator : MonoBehaviour
{
    private Animator animator;

    private int currentDirection = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FaceDirection(Vector2 direction)
    {
        int newDirection;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                newDirection = 2; // Right
            }
            else
            {
                newDirection = 1; // Left
            }
        }
        else
        {
            if (direction.y > 0)
            {
                newDirection = 3; // Up
            }
            else
            {
                newDirection = 0; // Down
            }
        }

        if (newDirection == currentDirection)
            return;

        currentDirection = newDirection;

        animator.SetInteger("Direction", currentDirection);
    }

    public void FacePlayer(Transform player, Transform npc)
    {
        if (player == null || npc == null)
            return;

        Vector2 direction = player.position - npc.position;

        FaceDirection(direction);
    }
}