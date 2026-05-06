using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine; 

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb; // Replaced CharacterController with Rigidbody

    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineVirtualCamera lockOnCam;
    [SerializeField] private CinemachineTargetGroup targetGroup;

    [Header("Movement")]
    // moveSpeed is gone because Root Motion controls the speed natively
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Lock-On")]
    [SerializeField] private float lockOnRadius = 10f;
    [SerializeField] private LayerMask enemyLayer;

    private PlayerInputs inputs;
    private Vector2 moveInput;

    private Transform currentTarget;
    private bool isLockedOn;

    private void Start()
    {
        // 1. Grab inputs from your centralized handler
        inputs = GetComponent<PlayerInputHandler>().Inputs;
        
        // 2. Subscribe to the Lock-On button
        inputs.Combat.LockOn.performed += OnLockOn;
    }

    private void OnDestroy()
    {
        // 3. Clean up the subscription when the script is destroyed
        if (inputs != null)
        {
            inputs.Combat.LockOn.performed -= OnLockOn;
        }
    }

    private void Update()
    {
        ReadInput();
        HandleRotation(); // Movement is handled by Root Motion, so we only handle Rotation now!
        UpdateAnimator();
    }

    // ------------------------
    // INPUT
    // ------------------------
    private void ReadInput()
    {
        moveInput = inputs.Movement.Run.ReadValue<Vector2>();
    }

    private Vector3 GetCameraRelativeDirection()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * moveInput.y + right * moveInput.x;
    }

    // ------------------------
    // ROTATION
    // ------------------------
    private void HandleRotation()
    {
        if (isLockedOn && currentTarget != null)
        {
            RotateTowards(currentTarget.position);
        }
        else
        {
            Vector3 move = GetCameraRelativeDirection();
            if (move.sqrMagnitude > 0.01f)
            {
                RotateTowards(transform.position + move);
            }
        }
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    // ------------------------
    // ANIMATION
    // ------------------------
    private void UpdateAnimator()
    {
        if (isLockedOn)
        {
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);
        }
        else
        {
            animator.SetFloat("Speed", moveInput.magnitude);
        }

        animator.SetBool("IsLockedOn", isLockedOn);
    }

    // ------------------------
    // LOCK-ON SYSTEM
    // ------------------------
    private void OnLockOn(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        
        if (isLockedOn)
        {
            ClearTarget();
        }
        else
        {
            FindTarget();
        }
    }

    private void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            lockOnRadius,
            enemyLayer
        );

        if (hits.Length == 0)
        {
            Debug.Log("No enemies in range.");
            return;
        } 

        float closestDistance = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                bestTarget = hit.transform;
            }
        }

        currentTarget = bestTarget;
        isLockedOn = true;

        if (lockOnCam != null) lockOnCam.Priority = 11; 
        
        if (targetGroup != null && targetGroup.m_Targets.Length > 1)
        {
            targetGroup.m_Targets[1].target = currentTarget;
        }
    }

    private void ClearTarget()
    {
        currentTarget = null;
        isLockedOn = false;

        if (lockOnCam != null) lockOnCam.Priority = 9; 
        
        if (targetGroup != null && targetGroup.m_Targets.Length > 1)
        {
            targetGroup.m_Targets[1].target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lockOnRadius);
    }
}