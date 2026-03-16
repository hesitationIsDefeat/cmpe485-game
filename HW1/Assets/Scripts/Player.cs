using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    [Header("Movement Settings")]
    public float moveSpeed = 40f;
    public float dragValue = 2f;

    private Rigidbody rigidBody;
    private Vector2 inputVector;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        rigidBody.freezeRotation = true;

        rigidBody.drag = dragValue;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw(HORIZONTAL);
        float z = Input.GetAxisRaw(VERTICAL);
        inputVector = new Vector2(x, z).normalized;
    }

    void FixedUpdate() {
        HandleMovement();
    }

    void HandleMovement() {
        Vector3 moveDirection = transform.right * inputVector.x + transform.forward * inputVector.y;

        rigidBody.AddForce(moveDirection * moveSpeed, ForceMode.Acceleration);
    }
}
