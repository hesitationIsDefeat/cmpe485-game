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

    private void Start()
    {
        animator = GetComponent<Animator>();
        inputs = GetComponent<PlayerInputHandler>().Inputs;
        playerMovement = GetComponent<PlayerMovement>();

        // Punches
        inputs.Combat.Jab.performed += context => HandleLeftClick();
        inputs.Combat.Cross.performed += context => HandleRightClick();
        inputs.Combat.LeftHook.performed += context => animator.SetTrigger(Constants.Animations.TriggerLeftHook);
        inputs.Combat.RightHook.performed += context => animator.SetTrigger(Constants.Animations.TriggerRightHook);
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

    // private void OnEnable() => inputs.Enable();
    // private void OnDisable() => inputs.Disable();

    // private void HandleMoveLeft()
    // {
    //     bool isDefending = inputs.Combat.ModifierKey.IsPressed(); 

    //     if (isDefending)
    //     {
    //         animator.SetTrigger(Constants.Animations.TriggerLeftPivot);
    //     }
    //     else
    //     {
    //         //animator.SetTrigger("StepLeft"); 
    //     }
    // }

    // private void HandleMoveRight()
    // {
    //     bool isDefending = inputs.Combat.ModifierKey.IsPressed();
    //     Debug.Log(isDefending); 

    //     if (isDefending)
    //     {
    //         Debug.Log("Right pivot");
    //         animator.SetTrigger(Constants.Animations.TriggerRightPivot);
    //     }
    //     else
    //     {
    //         //animator.SetTrigger("StepLeft"); 
    //     }
    // }

    private void OnMovementInput(InputAction.CallbackContext ctx)
    {
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
