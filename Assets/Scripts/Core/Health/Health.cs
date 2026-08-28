using System;
using UnityEngine;
using Arkanor.Combat;

namespace Arkanor.Characters
{

    public class Health : MonoBehaviour
    {
        [SerializeField]
        private CharacterStats stats;

        private Knockback knockback;

        private int currentHealth;

        public int CurrentHealth => currentHealth;
        public int MaxHealth =>
            stats != null
                ? (int)stats.MaxHealth.Value
                : 0;
        public bool IsDead => currentHealth <= 0;

        public event Action<int, int> OnHealthChanged;
        public event Action OnDeath;


        private void Awake()
        {
            if (stats == null)
                stats = GetComponent<CharacterStats>();

            knockback = GetComponent<Knockback>();

            if (stats == null)
            {
                Debug.LogError(
                    $"{gameObject.name} nie posiada komponentu CharacterStats."
                );

                return;
            }

            stats.Initialize();

            currentHealth = (int)stats.MaxHealth.Value;
        }

        public void Damage(int damage, Vector2 hitDirection)
        {
            if (IsDead)
                return;

            int finalDamage = Mathf.Max(
                1,
                damage - (int)stats.Defense.Value
            );

            currentHealth = Mathf.Max(
                0,
                currentHealth - finalDamage
            );

            OnHealthChanged?.Invoke(
                currentHealth,
                (int)stats.MaxHealth.Value
            );

            Debug.Log(
                $"{gameObject.name} otrzymał {finalDamage} obrażeń"
            );

            if (knockback != null && !IsDead)
            {
                knockback.Apply(hitDirection);
            }

            if (IsDead)
                OnDeath?.Invoke();
        }

        public void Heal(int amount)
        {
            currentHealth = Mathf.Min(
                currentHealth + amount,
                (int)stats.MaxHealth.Value);

            OnHealthChanged?.Invoke(currentHealth, (int)stats.MaxHealth.Value);
        }
    }
}