using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvade : MonoBehaviour
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

        actions.Evade.LPivot.performed += context => PivotLeft();
        actions.Evade.RPivot.performed += context => PivotRight();
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    private void PivotLeft()
    {
        animator.SetTrigger(Constants.AnimTriggerLPivot);
    }

    private void PivotRight()
    {
        animator.SetTrigger(Constants.AnimTriggerRPivot);
    }
}
