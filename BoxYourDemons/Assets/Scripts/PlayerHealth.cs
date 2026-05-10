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
    private CharacterAudio charAudio;
    
    public bool IsBlocking { get; set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        charAudio = GetComponent<CharacterAudio>();
        currentHealth = maxHealth;
        
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public DamageResult TakeDamage(int damage)
    {
        if (IsBlocking)
        {
            if (charAudio != null) charAudio.PlayBlockSound();
            return DamageResult.Blocked;
        }

        currentHealth -= damage;
        
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        
        if (charAudio != null) charAudio.PlayHitSound();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null) animator.SetTrigger("Hit");
        }

        return DamageResult.Success;
    }
    
    private void Die()
    {}
}
