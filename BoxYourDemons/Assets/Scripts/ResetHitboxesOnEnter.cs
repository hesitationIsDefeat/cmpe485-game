using UnityEngine;

// Notice this doesn't inherit from MonoBehaviour!
public class ResetHitboxesOnEnter : StateMachineBehaviour 
{
    // This built-in function fires the exact millisecond the Animator enters a state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // The 'animator' parameter gives us direct access to the Player object
        PlayerCombat combatScript = animator.GetComponent<PlayerCombat>();
        
        if (combatScript != null)
        {
            combatScript.DisableAllHitboxes();
        }
    }
}
