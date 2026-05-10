using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    
    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject uiCanvasObject;
    
    [Header("Posture System")]
    [SerializeField] private int blocksToStun = 3;
    [SerializeField] private float stunDuration = 3f;
    
    [Header("Posture UI")]
    [SerializeField] private Transform postureBarContainer; 
    [SerializeField] private GameObject blockPipPrefab;     
    [SerializeField] private Image stunTimerCircle;      
    
    private int currentBlocksLeft;
    public bool IsMajorStunned { get; private set; } = false;

    private Animator animator;
    private CharacterAudio charAudio;
    private EnemyAI enemyAI; 
    private Collider pushbox;
    private Rigidbody rb;
    
    public bool IsBlocking { get; set; } = false;
    
    public bool IsDead { get; private set; } = false;
    public event Action OnDeath;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        charAudio = GetComponent<CharacterAudio>();
        enemyAI = GetComponent<EnemyAI>();
        pushbox = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        
        currentHealth = maxHealth;
        
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

	currentBlocksLeft = blocksToStun;
        stunTimerCircle.gameObject.SetActive(false);
        postureBarContainer.gameObject.SetActive(true);

        for (int i = 0; i < blocksToStun; i++)
        {
            Instantiate(blockPipPrefab, postureBarContainer);
        }
        
        SetHealthBarVisibility(false);
    }
    
    public void InitializeStats(int newHealth, int blocks, float stunTime)
    {
        maxHealth = newHealth;
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        blocksToStun = blocks;
        currentBlocksLeft = blocksToStun;
        stunDuration = stunTime;

        foreach (Transform child in postureBarContainer)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < blocksToStun; i++)
        {
            Instantiate(blockPipPrefab, postureBarContainer);
        }
    }
    
    public void SetHealthBarVisibility(bool isVisible)
    {
    	if (IsDead) 
        {
            if (uiCanvasObject != null) uiCanvasObject.SetActive(false);
            return;
        }
        
        if (uiCanvasObject != null)
        {
            uiCanvasObject.SetActive(isVisible);
        }
    }

    public DamageResult TakeDamage(int damage)
    {
    	if (IsDead) return DamageResult.Ignored;
    	
        if (IsBlocking)
        {
            if (charAudio != null) charAudio.PlayBlockSound();
            return DamageResult.Blocked;
        }

        currentHealth -= damage;

	if (healthSlider != null) healthSlider.value = currentHealth;
	
	if (charAudio != null) charAudio.PlayHitSound();
	
	if (enemyAI != null) enemyAI.ResetAttackCooldown();
	
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
    
    public void OnAttackBlocked()
    {
        if (IsDead || IsMajorStunned) return;

	if (enemyAI != null) enemyAI.ResetAttackCooldown();
	
        currentBlocksLeft--;
        
        if (currentBlocksLeft >= 0)
        {
            postureBarContainer.GetChild(currentBlocksLeft).gameObject.SetActive(false);
        }

        if (currentBlocksLeft <= 0)
        {
            StartCoroutine(MajorStunRoutine());
        }
        else
        {
            if (animator != null) animator.SetTrigger("Recoil");
        }
    }

    private System.Collections.IEnumerator MajorStunRoutine()
    {
        IsMajorStunned = true;

        if (animator != null) animator.SetTrigger("Hit"); 

        postureBarContainer.gameObject.SetActive(false);
        stunTimerCircle.gameObject.SetActive(true);
        stunTimerCircle.fillAmount = 1f;

        float timer = stunDuration;
        while (timer > 0)
        {
            if (IsDead) yield break; 
            
            timer -= Time.deltaTime;
            stunTimerCircle.fillAmount = timer / stunDuration;
            yield return null; 
        }

        currentBlocksLeft = blocksToStun;
        foreach (Transform pip in postureBarContainer) pip.gameObject.SetActive(true); 
        
        stunTimerCircle.gameObject.SetActive(false);
        postureBarContainer.gameObject.SetActive(true);
        
        IsMajorStunned = false;
    }

    private void Die()
    {
        IsDead = true;
        
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
        
        OnDeath?.Invoke();
    }
}
