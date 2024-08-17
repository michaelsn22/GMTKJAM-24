using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

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

    private int scaleCounter = 0; //int to track how many times we have been scaled. this is the best way i can think to handle when we get to high scale values.

    public CinemachineFreeLook freeLookCamera;

    //audio stuff
    [SerializeField] private AudioSource MainAudioSource;
    [SerializeField] private AudioClip slimeNoise;


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
        if (Input.GetKeyDown(KeyCode.Space) && CheckGroundDistance())//isGrounded
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
            MainAudioSource.PlayOneShot(slimeNoise);
        }

        //Debug.Log(CheckGroundDistance()); //check if we are considered to be touching the ground, including coyote time.
        //CheckGroundDistance2(); //check raw distance.
        //Debug.Log(rb.velocity.y); //check vertical velocity.
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
        //Debug.Log("jumping");
        rb.velocity = new Vector3(0, 0f, 0);
        //Debug.Log(rb.velocity.y);
        rb.AddForce(transform.up * burstValueUpwards, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        //we probably need to alter movement speed for the bigger slime object?
        
        MainAudioSource.PlayOneShot(slimeNoise);

        if (scaleCounter >= 16)
        {
            //lets just stop at this size for now...
            return;
        }
        string collidedObjectName = "";
        collidedObjectName = collision.gameObject.name;
        //Debug.Log("We collided with "+collision.gameObject.name);

        if (collidedObjectName.Contains("Plane"))
        {
            return;
        }

        // Calculate the new scale
        Vector3 newScale = gameObject.transform.localScale * (1f + scaleIncrease);
        
        // Apply the new scale
        transform.localScale = newScale;

        //Debug.Log("Object scaled up by 30%");

        scaleCounter++;
        //Debug.Log("scaleCounter = "+scaleCounter);

        //handle cam stuff
        if (scaleCounter <= 11)
        {
            // Increase the radius of the TopRig orbit
            freeLookCamera.m_Orbits[0].m_Radius += 2f;

            // Increase the radius of the MiddleRig orbit
            freeLookCamera.m_Orbits[1].m_Radius += 2f;

            // Increase the radius of the BottomRig orbit
            freeLookCamera.m_Orbits[2].m_Radius += 2f;

            return;
        }

        // Increase the radius of the TopRig orbit
        freeLookCamera.m_Orbits[0].m_Radius += 10f;

        // Increase the radius of the MiddleRig orbit
        freeLookCamera.m_Orbits[1].m_Radius += 10f;

        // Increase the radius of the BottomRig orbit
        freeLookCamera.m_Orbits[2].m_Radius += 10f;
    }


    //method to check if we are a certain distance from the ground.
    private bool CheckGroundDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheck.position, Vector3.down, out hit))
        {
            float distanceToGround = hit.distance;
            if (distanceToGround <= 0.4f || distanceToGround <= 2f && rb.velocity.y <= -13f)
            {
                //Debug.Log("Distance to ground is below 3f: " + distanceToGround);
                return true;
            }
            else
            {
                return false;
            }
        }
        else{
            return false;
        }
    }

    private void CheckGroundDistance2()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheck.position, Vector3.down, out hit))
        {
            float distanceToGround = hit.distance;
            //Debug.Log(distanceToGround);
        }
    }
}
