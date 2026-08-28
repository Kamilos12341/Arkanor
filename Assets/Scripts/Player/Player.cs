using UnityEngine;
using Arkanor.Characters;

namespace Arkanor.Player
{
    public class Player : MonoBehaviour
    {
        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();

            if (health == null)
            {
                Debug.LogError(
                    $"{gameObject.name} nie posiada komponentu Health."
                );

                return;
            }

            health.OnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.OnDeath -= OnDeath;
            }
        }

        private void OnDeath()
        {
            Debug.Log("Game Over");
        }
    }
}