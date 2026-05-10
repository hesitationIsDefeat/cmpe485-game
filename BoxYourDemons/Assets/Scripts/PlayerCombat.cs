using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Hitboxes")]
    [SerializeField] private Hitbox leftGlove;
    [SerializeField] private Hitbox rightGlove;

    public void EnableLeftGlove() => leftGlove.EnableHitbox();
    public void EnableRightGlove() => rightGlove.EnableHitbox();

    public void DisableLeftGlove() => leftGlove.DisableHitbox();
    public void DisableRightGlove() => rightGlove.DisableHitbox();
    
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


        inputs.Combat.Jab.performed += context => HandleLeftClick();
        inputs.Combat.Cross.performed += context => HandleRightClick();
        inputs.Combat.LeftHook.performed += context => HandleLeftHookClick(); 
        inputs.Combat.RightHook.performed += context => HandleRightHookClick();

        inputs.Movement.Run.performed += OnMovementInput;
    }

    private void OnDestroy()
    {
        if (inputs != null)
        {
            inputs.Movement.Run.performed -= OnMovementInput;
        }
    }
    
    private void HandleLeftHookClick() {
    	if (IsStunned || !playerMovement.InputEnabled) return;
    	
    	animator.SetTrigger(Constants.Animations.TriggerLeftHook);
    }
    

    private void HandleRightHookClick() {
    	if (IsStunned || !playerMovement.InputEnabled) return;
    	
    	animator.SetTrigger(Constants.Animations.TriggerRightHook);
    }

    private void OnMovementInput(InputAction.CallbackContext ctx)
    {
    	if (IsStunned || !playerMovement.InputEnabled) return;

        Vector2 moveDir = ctx.ReadValue<Vector2>();
        
        bool isDefending = inputs.Combat.ModifierKey.IsPressed();

        if (isDefending && playerMovement != null && playerMovement.IsLockedOn)
        {
            if (moveDir.x < -0.1f)
            {
                animator.SetTrigger(Constants.Animations.TriggerLeftPivot);
            }
            else if (moveDir.x > 0.1f)
            {
                animator.SetTrigger(Constants.Animations.TriggerRightPivot);
            }
        }
    }

    private void HandleLeftClick()
    {
    	if (IsStunned || !playerMovement.InputEnabled) return;

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
    	if (IsStunned || !playerMovement.InputEnabled) return;

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
