using UnityEngine;
using UnityEngine.UI;

namespace Arkanor.Characters
{
    public class HealthBarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Health health;
        [SerializeField] private Image fill;

        private void Start()
        {
            if (health == null)
                health = FindFirstObjectByType<Health>();

            if (health == null)
            {
                Debug.LogError(
                    "HealthBarUI nie znalazł komponentu Health."
                );

                return;
            }

            if (fill == null)
            {
                Debug.LogError(
                    "HealthBarUI nie ma przypisanego Fill."
                );

                return;
            }

            health.OnHealthChanged += UpdateHealthBar;

            UpdateHealthBar(
                health.CurrentHealth,
                health.MaxHealth
            );
        }

        private void OnDestroy()
        {
            if (health != null)
                health.OnHealthChanged -= UpdateHealthBar;
        }

        private void UpdateHealthBar(int currentHealth, int maxHealth)
        {
            if (maxHealth <= 0)
            {
                fill.fillAmount = 0f;
                return;
            }

            fill.fillAmount =
                (float)currentHealth / maxHealth;
        }
    }
}