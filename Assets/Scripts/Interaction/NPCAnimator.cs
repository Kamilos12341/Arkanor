using UnityEngine;

public class NPCAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FaceDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                animator.SetInteger("Direction", 2);
            }
            else
            {
                animator.SetInteger("Direction", 1);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                animator.SetInteger("Direction", 3);
            }
            else
            {
                animator.SetInteger("Direction", 0);
            }
        }
    }
}