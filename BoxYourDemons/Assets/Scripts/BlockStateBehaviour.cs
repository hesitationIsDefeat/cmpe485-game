using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStateBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Turn the absolute defense ON the exact frame the parry animation starts
        PlayerHealth health = animator.GetComponent<PlayerHealth>();
        if (health != null) health.IsBlocking = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Turn the defense OFF the exact frame the animation finishes
        PlayerHealth health = animator.GetComponent<PlayerHealth>();
        if (health != null) health.IsBlocking = false;
    }
}
