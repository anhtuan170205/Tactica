using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDie;
    public event EventHandler OnHealthChanged;
    private const int MAX_HEALTH = 100;
    [SerializeField] private int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        OnDie?.Invoke(this, EventArgs.Empty);
    }
    public float GetHealthNormalized()
    {
        return (float)health / MAX_HEALTH;
    }
}
