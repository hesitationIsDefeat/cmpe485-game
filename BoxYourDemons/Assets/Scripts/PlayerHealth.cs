using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    
    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    private Animator animator;
    private Rigidbody rb;
    private Collider col;
    private CharacterAudio charAudio;
    
    public bool IsBlocking { get; set; } = false;
    
      public bool IsDead { get; private set; } = false;
    public event Action OnDeath;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
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
            if (!IsDead) Die();
        }
        else
        {
            if (animator != null) animator.SetTrigger("Hit");
        }

        return DamageResult.Success;
    }
    
    private void Die()
    {
    	if (rb != null) rb.isKinematic = true;
    	if (col != null) col.enabled = false;
    	if (animator != null && !IsDead) animator.SetTrigger("Knockout");
    	IsDead = true;
    	gameObject.layer = LayerMask.NameToLayer("Default");
    	OnDeath?.Invoke();
    }
    
    public void Respawn()
    {
        IsDead = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
        currentHealth = maxHealth;
        if (healthSlider != null) healthSlider.value = currentHealth;
        if (rb != null) rb.isKinematic = false;
    	if (col != null) col.enabled = true;
        animator.Rebind();
        animator.Update(0f);
    }
}
