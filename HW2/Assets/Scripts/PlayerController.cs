using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public float moveSpeed = 5f;
    public float turnCooldown = 0.2f; // How long to disable turning (in seconds)
    
    private Rigidbody rb;
    private float moveInput;
    private float turnTimer = 0f; // Keeps track of the cooldown

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. Forward and backward movement stays continuous
        moveInput = Input.GetAxisRaw(VERTICAL);

        // 2. Cooldown Logic
        if (turnTimer > 0)
        {
            // If the timer is active, count down using real time
            turnTimer -= Time.deltaTime;
        }
        else
        {
            // If the timer is 0 or less, we are allowed to turn again
            float turnInput = Input.GetAxisRaw(HORIZONTAL);

            // If the player presses left (-1) or right (1)
            if (turnInput != 0)
            {
                // Determine the angle: 90 for right, -90 for left
                float angle = turnInput > 0 ? 90f : -90f;
                
                // Instantly snap the Rigidbody's rotation
                Quaternion turnRotation = Quaternion.Euler(0f, angle, 0f);
                rb.MoveRotation(rb.rotation * turnRotation);

                // Reset the timer to lock out turning for the next 1 second
                turnTimer = turnCooldown;
            }
        }
    }

    void FixedUpdate() 
    {
        // Apply the forward/backward movement exactly as before
        Vector3 forwardMovement = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMovement);
    }
}