using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunStateBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement pm = animator.GetComponent<PlayerMovement>();
        if (pm != null) pm.IsStunned = true;

        PlayerCombat pc = animator.GetComponent<PlayerCombat>();
        if (pc != null) pc.IsStunned = true;

    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement pm = animator.GetComponent<PlayerMovement>();
        if (pm != null) pm.IsStunned = false;

        PlayerCombat pc = animator.GetComponent<PlayerCombat>();
        if (pc != null) pc.IsStunned = false;
    }
}
