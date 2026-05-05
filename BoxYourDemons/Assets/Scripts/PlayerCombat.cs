using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    private PlayerInputs inputs;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputs = new PlayerInputs();

        // Punches
        inputs.Combat.Jab.performed += context => HandleLeftClick();
        inputs.Combat.Cross.performed += context => HandleRightClick();
        inputs.Combat.LeftHook.performed += context => animator.SetTrigger(Constants.Animations.TriggerLeftHook);
        inputs.Combat.RightHook.performed += context => animator.SetTrigger(Constants.Animations.TriggerRightHook);
    }

    private void OnEnable() => inputs.Enable();
    private void OnDisable() => inputs.Disable();

    private void HandleMoveLeft()
    {
        bool isDefending = inputs.Combat.ModifierKey.IsPressed(); 

        if (isDefending)
        {
            animator.SetTrigger(Constants.Animations.TriggerLeftPivot);
        }
        else
        {
            //animator.SetTrigger("StepLeft"); 
        }
    }

        private void HandleMoveRight()
    {
        bool isDefending = inputs.Combat.ModifierKey.IsPressed(); 

        if (isDefending)
        {
            animator.SetTrigger(Constants.Animations.TriggerRightPivot);
        }
        else
        {
            //animator.SetTrigger("StepLeft"); 
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
