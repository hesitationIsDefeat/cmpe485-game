using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum AIDifficulty { Easy, Medium, Hard }

    [Header("AI Settings")]
    public AIDifficulty currentDifficulty = AIDifficulty.Easy;
    
    [Header("Targeting")]
    [SerializeField] private Transform player;
    [SerializeField] private float chaseRadius = 50f;        
    [SerializeField] private float stoppingDistance = 1.2f; 

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Combat Settings")]
    [SerializeField] public float attackCooldown = 2.0f;
    private float nextAttackTime = 0f;
    
    [Header("Hitboxes")]
    [SerializeField] private Hitbox leftGlove;
    [SerializeField] private Hitbox rightGlove;

    private readonly string[] easyPunches = { "Jab", "Cross", "LHook", "RHook" };

    private Rigidbody rb;
    private Animator animator;
    
    public void EnableLeftGlove() => leftGlove.EnableHitbox();
    public void EnableRightGlove() => rightGlove.EnableHitbox();

    public void DisableLeftGlove() => leftGlove.DisableHitbox();
    public void DisableRightGlove() => rightGlove.DisableHitbox();
    
    public void DisableAllHitboxes()
    {
        if (leftGlove != null) leftGlove.DisableHitbox();
        if (rightGlove != null) rightGlove.DisableHitbox();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        EnemyHealth myHealth = GetComponent<EnemyHealth>();

        if ((myHealth != null && myHealth.IsMajorStunned)) 
        {
            rb.velocity = Vector3.zero; 
            return; 
        }

        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(rb.position, player.position);

        if (distanceToPlayer <= chaseRadius && distanceToPlayer > stoppingDistance)
        {
            FacePlayer();
            MoveTowardsPlayer();
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            
            if (distanceToPlayer <= stoppingDistance)
            {
                FacePlayer();
                
                Vector3 directionToPlayer = (player.position - rb.position).normalized;
                directionToPlayer.y = 0; 
                
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

                if (angleToPlayer <= 20f)
                {
                    if (Time.time >= nextAttackTime)
                    {
                        ExecuteCombatLogic();
                    }
                }
            }
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - rb.position).normalized;
        direction.y = 0; 

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - rb.position).normalized;
        direction.y = 0; 
        
        Vector3 newPosition = rb.position + (direction * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }
    
    public void SetTarget(Transform targetPlayer)
    {
        player = targetPlayer;
        
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            
            directionToPlayer.y = 0; 
            
            if (directionToPlayer != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }
        }
    }

    public void ResetAttackCooldown()
    {
        nextAttackTime = Time.time + attackCooldown; 
    }

    private void ExecuteCombatLogic()
    {
        switch (currentDifficulty)
        {
            case AIDifficulty.Easy:
                PerformEasyAttack();
                break;
            case AIDifficulty.Medium:
                // TODO: Implement Medium Logic
                break;
            case AIDifficulty.Hard:
                // TODO: Implement Hard Logic
                break;
        }
    }

    private void PerformEasyAttack()
    {
        int randomIndex = Random.Range(0, easyPunches.Length);
        string selectedPunch = easyPunches[randomIndex];

        animator.SetTrigger(selectedPunch);

        float randomDelay = Random.Range(-0.2f, 0.5f); 
        nextAttackTime = Time.time + attackCooldown + randomDelay;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}
