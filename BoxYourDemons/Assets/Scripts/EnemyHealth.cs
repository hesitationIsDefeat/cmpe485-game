using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    
    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject uiCanvasObject;

    private Animator animator;
    private EnemyAI enemyAI; 
    private Collider pushbox;
    private Rigidbody rb;
    
    public bool IsBlocking { get; set; } = false;
    
    private bool isDead = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        pushbox = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        
        currentHealth = maxHealth;
        
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        SetHealthBarVisibility(false);
    }
    
    public void SetHealthBarVisibility(bool isVisible)
    {
    	if (isDead) 
        {
            if (uiCanvasObject != null) uiCanvasObject.SetActive(false);
            return;
        }
        
        if (uiCanvasObject != null)
        {
            uiCanvasObject.SetActive(isVisible);
        }
    }

    // This is the exact function your Hitbox script is trying to call
    public DamageResult TakeDamage(int damage)
    {
    	if (isDead) return DamageResult.Ignored;
    	
        if (IsBlocking)
        {
            return DamageResult.Blocked;
        }

        currentHealth -= damage;

	if (healthSlider != null) healthSlider.value = currentHealth;
	
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
    {
        isDead = true;
        
        if (rb != null) rb.isKinematic = true;

        if (animator != null) animator.SetTrigger("Knockout");

        SetHealthBarVisibility(false);

        if (enemyAI != null) enemyAI.enabled = false;

        if (pushbox != null) pushbox.enabled = false;

        Transform hurtbox = transform.Find("EnemyHurtbox");
        if (hurtbox != null)
        {
            hurtbox.gameObject.SetActive(false);
        }

        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
