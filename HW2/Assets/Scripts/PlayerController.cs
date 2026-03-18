using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public float moveSpeed = 5f;
    public float turnDuration = 0.2f; 
    
    private Rigidbody rb;
    private float moveInput;
    private bool isTurning = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw(VERTICAL);

        if (!isTurning)
        {
            float turnInput = Input.GetAxisRaw(HORIZONTAL);

            if (turnInput != 0)
            {
                float angle = turnInput > 0 ? 90f : -90f;
                
                StartCoroutine(SmoothTurn(angle));
            }
        }
    }

    void FixedUpdate() 
    {
        Vector3 forwardMovement = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMovement);
    }

    IEnumerator SmoothTurn(float angle)
    {
        isTurning = true;

        Quaternion startRotation = rb.rotation;
        Quaternion targetRotation = rb.rotation * Quaternion.Euler(0f, angle, 0f);
        
        float elapsedTime = 0f;

        while (elapsedTime < turnDuration)
        {
            rb.MoveRotation(Quaternion.Slerp(startRotation, targetRotation, elapsedTime / turnDuration));
            
            elapsedTime += Time.fixedDeltaTime;
            
            yield return new WaitForFixedUpdate(); 
        }

        rb.MoveRotation(targetRotation);
        
        isTurning = false; 
    }
}