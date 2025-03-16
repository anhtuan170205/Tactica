using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDie;
    //public event EventHandler OnHealthChanged;
    [SerializeField] private int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        OnDie?.Invoke(this, EventArgs.Empty);
    }
}
