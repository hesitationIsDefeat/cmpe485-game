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

        SetHealthBarVisibility(false);
    }
    
    public void SetHealthBarVisibility(bool isVisible)
    {
        if (uiCanvasObject != null)
        {
            uiCanvasObject.SetActive(isVisible);
        }
    }

    // This is the exact function your Hitbox script is trying to call
    public bool TakeDamage(int damage)
    {
        if (IsBlocking)
        {
            return false;
        }

        currentHealth -= damage;

	if (healthSlider != null) healthSlider.value = currentHealth;
	
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
