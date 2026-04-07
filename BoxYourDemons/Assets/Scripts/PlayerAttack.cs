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
        // Get the Animator component attached to the boxer
        animator = GetComponent<Animator>();
        
        // Initialize our actions
        actions = new PlayerActions();

        // When the 'Jab' action is performed (button pressed), run the ThrowJab function
        actions.Attack.Jab.performed += context => ThrowJab();
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
        // Trigger the 'Punch' parameter we created in the Animator
        animator.SetTrigger("Jab");
    }
}
