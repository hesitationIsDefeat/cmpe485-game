using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private PlayerInputs inputs;
    private Vector2 moveInput;

    // Lock-On Variables
    public Transform currentEnemyTarget; // Drag your enemy into this slot in the Inspector for now
    public Transform cameraTransform;    // Drag your Main Camera here
    private bool isLockedOn = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputs = new PlayerInputs();

        // Toggle Lock On
        inputs.Combat.LockOn.performed += ctx => ToggleLockOn();
    }

    private void OnEnable() => inputs.Enable();
    private void OnDisable() => inputs.Disable();

    private void Update()
    {
        // 1. Read the raw movement input (WASD or Joystick)
        moveInput = inputs.Movement.Run.ReadValue<Vector2>();

        // 2. Handle Keyboard Walk Modifier
        float currentSpeed = moveInput.magnitude;
        // if (inputs.Combat.WalkModifier.IsPressed())
        // {
        //     currentSpeed *= 0.5f; // Cut speed in half if holding the walk button
        // }

        // 3. Execute logic based on our current State
        if (isLockedOn && currentEnemyTarget != null)
        {
            HandleLockedOnMovement(currentSpeed);
        }
        else
        {
            HandleFreeMovement(currentSpeed);
        }
    }

    private void HandleFreeMovement(float speed)
    {
        // Tell the animator how fast we are going
        animator.SetFloat("Speed", speed);

        // Standard Free Roam Rotation: Point the character in the direction of the joystick
        if (moveInput != Vector2.zero)
        {
            Vector3 movementDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void HandleLockedOnMovement(float speed)
    {
        // 1. Send the specific X and Y inputs to the Animator's 2D Blend Tree
        animator.SetFloat("InputX", moveInput.x * speed);
        animator.SetFloat("InputY", moveInput.y * speed);

        // 2. Force the player to rotate and look directly at the enemy
        Vector3 directionToEnemy = currentEnemyTarget.position - transform.position;
        directionToEnemy.y = 0; // Keep the character from tilting up/down
        
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);

        // 3. Force the Camera to look at the enemy too (Simple version)
        if (cameraTransform != null)
        {
            // Position the camera behind the player and look at the enemy
            Vector3 camPosition = transform.position - (directionToEnemy.normalized * 3f) + (Vector3.up * 2f);
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, camPosition, Time.deltaTime * 5f);
            cameraTransform.LookAt(currentEnemyTarget.position + Vector3.up); // Look at their chest/head
        }
    }

    private void ToggleLockOn()
    {
        isLockedOn = !isLockedOn;
        animator.SetBool("IsLockedOn", isLockedOn);
    }
}
