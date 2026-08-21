using Arkanor.Characters;
using System;
using UnityEngine;

namespace Arkanor.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private string enemyId = "enemy";

        public string EnemyId => enemyId;

        public static event Action<string> OnDied;

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

            health.OnDeath += Die;
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.OnDeath -= Die;
            }
        }

        private void Die()
        {
            OnDied?.Invoke(enemyId);

            Destroy(gameObject);
        }
    }
}