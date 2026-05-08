using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Hitboxes")]
    [SerializeField] private Hitbox leftGlove;
    [SerializeField] private Hitbox rightGlove;

    // We will call these exactly when the fist extends
    public void EnableLeftGlove() => leftGlove.EnableHitbox();
    public void EnableRightGlove() => rightGlove.EnableHitbox();

    // We will call these exactly when the fist pulls back
    public void DisableLeftGlove() => leftGlove.DisableHitbox();
    public void DisableRightGlove() => rightGlove.DisableHitbox();
    
    // Safety net: Turn both off at the exact same time
    public void DisableAllHitboxes()
    {
        leftGlove.DisableHitbox();
        rightGlove.DisableHitbox();
    }
    private Animator animator;
    private PlayerInputs inputs;
    private PlayerMovement playerMovement;
    
    public bool IsStunned { get; set; } = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        inputs = GetComponent<PlayerInputHandler>().Inputs;
        playerMovement = GetComponent<PlayerMovement>();

        // Punches
        inputs.Combat.Jab.performed += context => HandleLeftClick();
        inputs.Combat.Cross.performed += context => HandleRightClick();
        inputs.Combat.LeftHook.performed += context => HandleLeftHookClick(); 
        inputs.Combat.RightHook.performed += context => HandleRightHookClick();
        // Pivots
        inputs.Movement.Run.performed += OnMovementInput;
    }

    private void OnDestroy()
    {
        // Always clean up subscriptions!
        if (inputs != null)
        {
            inputs.Movement.Run.performed -= OnMovementInput;
        }
    }
    
    private void HandleLeftHookClick() {
    	if (IsStunned) return;
    	
    	animator.SetTrigger(Constants.Animations.TriggerLeftHook);
    }
    

    private void HandleRightHookClick() {
    	if (IsStunned) return;
    	
    	animator.SetTrigger(Constants.Animations.TriggerRightHook);
    }

    private void OnMovementInput(InputAction.CallbackContext ctx)
    {
    	if (IsStunned) return;
        // 1. Read the WASD input
        Vector2 moveDir = ctx.ReadValue<Vector2>();
        
        // 2. Check if the player is holding the Shift/Modifier key
        bool isDefending = inputs.Combat.ModifierKey.IsPressed();

        // 3. If they are holding the modifier, intercept the movement!
        if (isDefending && playerMovement != null && playerMovement.IsLockedOn)
        {
            // If pressing A (Left)
            if (moveDir.x < -0.1f)
            {
                Debug.Log("Left pivot triggered");
                animator.SetTrigger(Constants.Animations.TriggerLeftPivot);
            }
            // If pressing D (Right)
            else if (moveDir.x > 0.1f)
            {
                Debug.Log("Right pivot triggered");
                animator.SetTrigger(Constants.Animations.TriggerRightPivot);
            }
        }
    }

    private void HandleLeftClick()
    {
    	if (IsStunned) return;
        // Check our universal Modifier stance
        bool isDefending = inputs.Combat.ModifierKey.IsPressed(); 

        if (isDefending)
        {
            animator.SetTrigger(Constants.Animations.TriggerLeftBlock);
        }
        else
        {
            animator.SetTrigger(Constants.Animations.TriggerJab);
        }
    }

    private void HandleRightClick()
    {
    	if (IsStunned) return;
        // Check our universal Modifier stance
        bool isDefending = inputs.Combat.ModifierKey.IsPressed(); 

        if (isDefending)
        {
            animator.SetTrigger(Constants.Animations.TriggerRightBlock);
        }
        else
        {
            animator.SetTrigger(Constants.Animations.TriggerCross);
        }
    }
}
