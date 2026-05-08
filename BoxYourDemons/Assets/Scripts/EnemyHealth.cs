using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private Animator animator;
    
    public bool IsBlocking { get; set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // This is the exact function your Hitbox script is trying to call
    public bool TakeDamage(int damage)
    {
        if (IsBlocking)
        {
            Debug.Log($"{gameObject.name} BLOCKED the attack!");

            return false;
        }

        currentHealth -= damage;
        Debug.Log($"<color=red>HIT!</color> {gameObject.name} Health: {currentHealth}");

        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} Knocked Out!");
        }

        return true;
    }

    private void Die()
    {
        Debug.Log("Enemy Knocked Out!");
        // animator.SetTrigger("Knockout");
    }
}
