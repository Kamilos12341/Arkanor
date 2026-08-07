using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private CharacterStats stats;

    private int currentHealth;

    public int CurrentHealth => currentHealth;

    public bool IsDead => currentHealth <= 0;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;


    private void Awake()
    {
        if (stats == null)
            stats = GetComponent<CharacterStats>();

    }

    private void Start()
    {
        currentHealth = (int)stats.MaxHealth.Value;
    }

    public void Damage(int damage)
    {
        int finalDamage = Mathf.Max(1, damage - (int)stats.Defense.Value);

        currentHealth = Mathf.Max(0, currentHealth - finalDamage);

        OnHealthChanged?.Invoke(currentHealth, (int)stats.MaxHealth.Value);

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