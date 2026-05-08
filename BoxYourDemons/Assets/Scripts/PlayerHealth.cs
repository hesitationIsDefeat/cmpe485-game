using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    
    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    private Animator animator;
    
    public bool IsBlocking { get; set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public bool TakeDamage(int damage)
    {
        if (IsBlocking)
        {
            return false;
        }

        currentHealth -= damage;
        
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

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
}
