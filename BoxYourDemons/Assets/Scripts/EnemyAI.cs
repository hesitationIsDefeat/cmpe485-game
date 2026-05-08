using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private Transform player;
    [SerializeField] private float chaseRadius = 5f;        // How close before they notice you
    [SerializeField] private float stoppingDistance = 1.2f; // Punching range (stop moving)

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;

    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Auto-find the player if you forget to drag them into the Inspector
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // How far away is the player?
        float distanceToPlayer = Vector3.Distance(rb.position, player.position);

        // If the player is inside the "aggro" radius, but not close enough to punch yet...
        if (distanceToPlayer <= chaseRadius && distanceToPlayer > stoppingDistance)
        {
            FacePlayer();
            MoveTowardsPlayer();
            
            // Tell the Animator to play the walking animation
            animator.SetBool("IsMoving", true);
        }
        else
        {
            // Either the player ran away, or we are close enough to punch. Stop walking!
            animator.SetBool("IsMoving", false);
            
            // If we are in punching range, keep staring at them even while stopped!
            if (distanceToPlayer <= stoppingDistance)
            {
                FacePlayer();
            }
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - rb.position).normalized;
        direction.y = 0; // CRITICAL: Keeps the enemy from tilting up or down!

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // Smoothly rotate the Rigidbody towards the player
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - rb.position).normalized;
        direction.y = 0; // Don't try to fly or dig underground!
        
        // Physically push the Rigidbody forward
        Vector3 newPosition = rb.position + (direction * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    // BONUS: Draws visual circles in the Unity Editor so you can easily see your ranges!
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}
