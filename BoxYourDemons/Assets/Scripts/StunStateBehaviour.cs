using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunStateBehaviour : StateMachineBehaviour
{
    // Fires the exact frame the "Getting Hit" animation begins
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Try to stun the Player
        PlayerMovement pm = animator.GetComponent<PlayerMovement>();
        if (pm != null) pm.IsStunned = true;

        PlayerCombat pc = animator.GetComponent<PlayerCombat>();
        if (pc != null) pc.IsStunned = true;

        // Try to stun the Enemy AI
        // EnemyAI ai = animator.GetComponent<EnemyAI>();
        // if (ai != null) ai.IsStunned = true;
    }

    // Fires the exact frame the "Getting Hit" animation finishes (or gets interrupted)
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement pm = animator.GetComponent<PlayerMovement>();
        if (pm != null) pm.IsStunned = false;

        PlayerCombat pc = animator.GetComponent<PlayerCombat>();
        if (pc != null) pc.IsStunned = false;

        // EnemyAI ai = animator.GetComponent<EnemyAI>();
        // if (ai != null) ai.IsStunned = false;
    }
}
