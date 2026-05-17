using UnityEngine;

public class ResetHitboxesOnEnter : StateMachineBehaviour 
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCombat combatScript = animator.GetComponent<PlayerCombat>();
        
        if (combatScript != null)
        {
            combatScript.DisableAllHitboxes();
        }
    }
}
