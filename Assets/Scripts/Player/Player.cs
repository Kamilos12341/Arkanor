using UnityEngine;

public class Player : MonoBehaviour
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();

        health.OnDeath += OnDeath;
    }

    private void OnDestroy()
    {
        health.OnDeath -= OnDeath;
    }

    private void OnDeath()
    {
        Debug.Log("Game Over");
    }
}