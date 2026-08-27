using Arkanor.Characters;
using Arkanor.Combat;
using Arkanor.Player;
using UnityEngine;

namespace Arkanor.Enemies
{
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

        private Knockback knockback;

        private Vector2 movement;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            stats = GetComponent<CharacterStats>();
            knockback = GetComponent<Knockback>();
        }

        private void Start()
        {
            if (PlayerReference.Instance == null)
            {
                Debug.LogWarning(
                    "Wolf nie znalazł PlayerReference."
                );

                return;
            }

            player = PlayerReference.Instance.Transform;
        }

        private void Update()
        {
            if (player == null)
            {
                movement = Vector2.zero;
                return;
            }

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

        private void FixedUpdate()
        {
            if (knockback != null && knockback.IsKnockedBack)
                return;

            rb.linearVelocity =
                movement * stats.MoveSpeed.Value;
        }

        private void MoveTowardsPlayer()
        {
            Vector2 direction =
                (player.position - transform.position).normalized;

            movement = direction;

            animator.SetBool("IsMoving", true);

            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x > 0;
            }
        }

        private void StopMoving()
        {
            movement = Vector2.zero;

            animator.SetBool("IsMoving", false);
        }

        private void TryAttack()
        {
            if (attackTimer > 0f)
                return;

            attackTimer = attackCooldown;

            Health playerHealth =
                player.GetComponent<Health>();

            if (playerHealth == null)
            {
                Debug.LogWarning(
                    "Player nie posiada komponentu Health."
                );

                return;
            }

            Vector2 hitDirection =
                (player.position - transform.position).normalized;

            playerHealth.Damage(
                (int)stats.Attack.Value,
                hitDirection
            );

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
    }
}