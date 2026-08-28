using UnityEngine;

namespace Arkanor.Combat
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Knockback : MonoBehaviour
    {
        [SerializeField] private float force = 4f;
        [SerializeField] private float duration = 0.15f;

        private Rigidbody2D rb;
        private float timer;

        public bool IsKnockedBack => timer > 0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (timer <= 0f)
                return;

            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        public void Apply(Vector2 direction)
        {
            if (direction == Vector2.zero)
                return;

            direction.Normalize();

            timer = duration;
            rb.linearVelocity = direction * force;
        }
    }
}