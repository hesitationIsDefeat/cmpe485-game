using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine; 

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb; 

    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineVirtualCamera lockOnCam;
    [SerializeField] private CinemachineTargetGroup targetGroup;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Lock-On")]
    [SerializeField] private float lockOnRadius = 10f;
    [SerializeField] private LayerMask enemyLayer;

    private PlayerInputs inputs;
    private Vector2 moveInput;

    private Transform currentTarget;
    private bool isLockedOn;
    public bool IsLockedOn => isLockedOn;
    
    private EnemyHealth currentTargetHealth;
    
    public bool IsStunned { get; set; } = false;

    private void Start()
    {
        inputs = GetComponent<PlayerInputHandler>().Inputs;
        
        inputs.Combat.LockOn.performed += OnLockOn;
    }

    private void OnDestroy()
    {
        if (inputs != null)
        {
            inputs.Combat.LockOn.performed -= OnLockOn;
        }
    }

    private void Update()
    {
        ReadInput();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
    	if (IsStunned) return;
    
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (inputs.Combat.ModifierKey.IsPressed() && isLockedOn) return;

        if (!isLockedOn && moveInput.sqrMagnitude > 0.01f)
        {
            if (currentTarget != null)
            {
                Vector3 strafeDirection = (transform.right * moveInput.x) + (transform.forward * moveInput.y);
                Vector3 newPosition = rb.position + (strafeDirection.normalized * moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);
            }
            else
            {
                Vector3 moveDirection = GetCameraRelativeDirection();
                Vector3 newPosition = rb.position + (moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);
            }
        }
    }

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

    private void OnLockOn(InputAction.CallbackContext ctx)
    {
        // if (!ctx.started) return;

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
            return;
        }

        float closestDistance = Mathf.Infinity;
        Transform bestTarget = null;
        EnemyHealth bestTargetHealth = null;

        foreach (var hit in hits)
        {
            EnemyHealth healthScript = hit.GetComponentInParent<EnemyHealth>();
            
            if (healthScript != null)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    // Lock onto the ROOT object, not the child collider
                    bestTarget = healthScript.transform; 
                    bestTargetHealth = healthScript;
                }
            }
        }
        
        if (bestTarget == null) return;

	if (currentTargetHealth != null)
        {
            currentTargetHealth.SetHealthBarVisibility(false);
        }

        currentTarget = bestTarget;
        currentTargetHealth = bestTargetHealth;
        isLockedOn = true;
        
        currentTargetHealth = currentTarget.GetComponent<EnemyHealth>();
        Debug.Log(currentTargetHealth);
        
        if (currentTargetHealth != null)
        {
            currentTargetHealth.SetHealthBarVisibility(true);
            Debug.Log("Enable health bar");
        }

        if (lockOnCam != null) lockOnCam.Priority = 11; 
        
        if (targetGroup != null && targetGroup.m_Targets.Length > 1)
        {
            targetGroup.m_Targets[1].target = currentTarget;
        }
    }

    private void ClearTarget()
    {
	if (currentTargetHealth != null)
        {
            currentTargetHealth.SetHealthBarVisibility(false);
            Debug.Log("Disable health bar");
        }
        
        currentTargetHealth = null;
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
