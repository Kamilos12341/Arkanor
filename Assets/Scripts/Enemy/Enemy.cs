using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();

        health.OnDeath += Die;
    }

    private void OnDestroy()
    {
        health.OnDeath -= Die;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}