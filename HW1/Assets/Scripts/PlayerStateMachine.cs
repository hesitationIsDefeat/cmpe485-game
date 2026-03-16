using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;
    
    private const string IS_WALKING_PARAM = "isWalking";
    private static readonly int IS_WALKING_HASH = Animator.StringToHash(IS_WALKING_PARAM);
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimationStates();
    }

    void HandleAnimationStates() {
        Vector3 horizontalVelocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        bool isWalking = currentSpeed > 0.2f;

        playerAnimator.SetBool(IS_WALKING_HASH, isWalking);
    }
}
