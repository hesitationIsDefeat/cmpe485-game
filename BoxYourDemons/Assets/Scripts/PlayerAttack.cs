using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private PlayerActions actions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        actions = new PlayerActions();

        actions.Attack.Jab.performed += context => ThrowJab();
        actions.Attack.LHook.performed += context => ThrowLeftHook();
        actions.Attack.RHook.performed += context => ThrowRightHook();
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    private void ThrowJab()
    {
        animator.SetTrigger(Constants.AnimTriggerJab);
    }

    private void ThrowLeftHook()
    {
        animator.SetTrigger(Constants.AnimTriggerLHook);
    }

    private void ThrowRightHook()
    {
        animator.SetTrigger(Constants.AnimTriggerRHook);
    }
}
