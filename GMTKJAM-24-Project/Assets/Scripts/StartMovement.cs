using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartMovement : MonoBehaviour
{
    private Rigidbody rb;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    public Transform cam;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float speed = 6f;
    public float burstValueUpwards = 20f;

    [SerializeField] private LayerMask groundMask;

    private Vector3 movement;
    private bool jumpRequested = false;

    //scaling vars
    public float scaleIncrease = 0.3f; // 30% increase
    private Vector3 originalScale;
    private bool hasScaled = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Input detection
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movement = new Vector3(horizontal, 0f, vertical).normalized;

        // Jump input detection
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void FixedUpdate()
    {
        MovePlayer();
        if (jumpRequested)
        {
            Jump();
            jumpRequested = false;
        }
    }

    void MovePlayer()
    {
        if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            rb.velocity = new Vector3(moveDir.x * speed, rb.velocity.y, moveDir.z * speed);
        }
        else //DECIDE WHETHER TO KEEP MOMENTUM OR NOT!
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void Jump()
    {
        Debug.Log("jumping");
        rb.AddForce(transform.up * burstValueUpwards, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        string collidedObjectName = "";
        collidedObjectName = collision.gameObject.name;
        Debug.Log("We collided with "+collision.gameObject.name);

        if (collidedObjectName.Contains("Plane"))
        {
            return;
        }

        // Calculate the new scale
        Vector3 newScale = gameObject.transform.localScale * (1f + scaleIncrease);
        
        // Apply the new scale
        transform.localScale = newScale;
        
        // Mark as scaled to prevent multiple scales
        hasScaled = true;

        Debug.Log("Object scaled up by 30%");
    }
}
