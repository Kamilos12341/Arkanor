using UnityEngine;

public class Wolf : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRange = 5f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackCooldown = 1.5f;

    private float attackTimer;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private CharacterStats stats;

    private Health health;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        stats = GetComponent<CharacterStats>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Wolf nie znalazł Playera.");
        }

        health.OnDeath += Die;
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDeath -= Die;
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance =
            Vector2.Distance(transform.position, player.position);

        attackTimer -= Time.deltaTime;

        if (distance <= attackRange)
        {
            StopMoving();
            TryAttack();
        }
        else if (distance <= detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopMoving();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction =
            (player.position - transform.position).normalized;

        rb.linearVelocity =
            direction * stats.MoveSpeed.Value;

        animator.SetBool("IsMoving", true);

        if (direction.x != 0)
        {
            spriteRenderer.flipX = direction.x > 0;
        }
    }

    private void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;

        animator.SetBool("IsMoving", false);
    }

    private void TryAttack()
    {
        if (attackTimer > 0f)
            return;

        Health playerHealth =
            player.GetComponent<Health>();

        if (playerHealth == null)
        {
            Debug.LogWarning(
                "Player nie posiada komponentu Health."
            );

            return;
        }

        playerHealth.Damage(
            (int)stats.Attack.Value
        );

        attackTimer = attackCooldown;

        Debug.Log(
            $"Wolf zaatakował gracza za {(int)stats.Attack.Value} obrażeń."
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }

    private void Die()
    {
        rb.linearVelocity = Vector2.zero;

        Debug.Log("Wolf umarł.");

        Destroy(gameObject);
    }
}