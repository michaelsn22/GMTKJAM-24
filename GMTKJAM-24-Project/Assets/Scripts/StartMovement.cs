using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartMovement : MonoBehaviour
{
    private float horizontalVal;
    private float verticalVal;
    private Rigidbody rb;
    public float burstValue = 20f;
    public bool isGrounded; // Variable to check if the character is on the ground
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    [SerializeField] private LayerMask groundMask;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        movePlayer();
    }

    private void movePlayer()
    {
        horizontalVal = Input.GetAxis("Horizontal");
        verticalVal = Input.GetAxis("Vertical");

       //slime moving?
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("jumping");
            rb.AddForce(transform.up * burstValue, ForceMode.Impulse);
        }

        Vector3 movementVector = new Vector3(horizontalVal, 0f, verticalVal);

        rb.AddForce(movementVector * burstValue);
    }
}
