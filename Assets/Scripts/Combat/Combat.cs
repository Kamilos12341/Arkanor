using UnityEngine;

public class Combat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackWidth = 0.8f;
    [SerializeField] private float attackCooldown = 0.5f;

    [Header("Target")]
    [SerializeField] private LayerMask targetLayer;

    private float attackTimer;

    private CharacterStats stats;
    private PlayerMovement playerMovement;
    private PlayerInputHandler input;

    private Vector2 facingDirection = Vector2.down;

    //private Animator animator;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        playerMovement = GetComponent<PlayerMovement>();
        input = GetComponent<PlayerInputHandler>();

        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateFacingDirection();

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        if (input.Attack.WasPressedThisFrame())
        {
            TryAttack();
        }
    }

    private void UpdateFacingDirection()
    {
        Vector2 movement = playerMovement.Movement;

        if (movement != Vector2.zero)
        {
            facingDirection = movement.normalized;
        }
    }

    private void TryAttack()
    {
        if (attackTimer > 0f)
            return;

        attackTimer = attackCooldown;

        Vector2 attackCenter =
            (Vector2)transform.position +
            facingDirection * attackRange * 0.5f;

        Vector2 attackSize = new Vector2(
            attackWidth,
            attackRange
        );

        if (Mathf.Abs(facingDirection.x) >
            Mathf.Abs(facingDirection.y))
        {
            attackSize = new Vector2(
                attackRange,
                attackWidth
            );
        }

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            attackCenter,
            attackSize,
            0f,
            targetLayer
        );

        foreach (Collider2D hit in hits)
        {
            Health targetHealth =
                hit.GetComponent<Health>();

            if (targetHealth == null)
                continue;

            targetHealth.Damage(
                (int)stats.Attack.Value
            );

            Debug.Log(
                $"{gameObject.name} zaatakował " +
                $"{hit.gameObject.name} za " +
                $"{(int)stats.Attack.Value} obrażeń."
            );
        }

        // animator.SetTrigger("Attack");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 direction = facingDirection;

        Vector2 attackCenter =
            (Vector2)transform.position +
            direction * attackRange * 0.5f;

        Vector2 attackSize = new Vector2(
            attackWidth,
            attackRange
        );

        if (Mathf.Abs(direction.x) >
            Mathf.Abs(direction.y))
        {
            attackSize = new Vector2(
                attackRange,
                attackWidth
            );
        }

        Gizmos.DrawWireCube(
            attackCenter,
            attackSize
        );
    }
}