using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class StartMovement : MonoBehaviour
{
    private Rigidbody rb;
    public bool isGrounded;
    public bool isGroundedCenter;
    public bool isGroundedLeft;
    public bool isGroundedRight;
    public Transform groundCheckCenter, groundCheckLeft, groundCheckRight;
    public float groundDistance = 0.4f;
    public Transform cam;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float speed = 6f;
    public int score = 0;
    public bool gameOver = false;
    public float burstValueUpwards = 20f;
    public float dashSpeed = 500f;
    private bool canDash = true;

    [SerializeField] private LayerMask groundMask;

    private Vector3 movement;
    private bool jumpRequested = false;
    private bool dashRequested = false;
    private bool isDashing = false;
    private float dashDuration = 0.2f; // Duration of the dash in seconds
    private float dashCooldown = 2f;

    //scaling vars
    public float scaleIncrease = 0.3f; // 30% increase
    private Vector3 originalScale;

    private int scaleCounter = 0; //int to track how many times we have been scaled. this is the best way i can think to handle when we get to high scale values.

    public CinemachineFreeLook freeLookCamera;

    //audio stuff
    [SerializeField] private AudioSource MainAudioSource;
    [SerializeField] private AudioClip slimeNoise;

    [SerializeField] private Material SlimeMaterial;


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
        if (Input.GetKeyDown(KeyCode.Space) && CheckGroundDistance() || Input.GetKeyDown(KeyCode.Space) && isGrounded)//isGrounded
        {
            jumpRequested = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)//isGrounded
        {
            dashRequested = true;
        }
        //dashRequested = !!Input.GetKeyDown(KeyCode.LeftShift);

        isGroundedCenter = Physics.CheckSphere(groundCheckCenter.position, groundDistance, groundMask);
        isGroundedLeft = Physics.CheckSphere(groundCheckLeft.position, groundDistance, groundMask);
        isGroundedRight = Physics.CheckSphere(groundCheckRight.position, groundDistance, groundMask);

        isGrounded = isGroundedCenter || isGroundedLeft || isGroundedRight;
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
        TryToDash();

        //Debug.Log(CheckGroundDistance()); //check if we are considered to be touching the ground, including coyote time.
        //CheckGroundDistance2(); //check raw distance.
        //Debug.Log(rb.velocity.y); //check vertical velocity.
    }

    void TryToDash()
    {
        if(!dashRequested)
        {
            return;
        }
        dashRequested = false;
        canDash = false;

        StartCoroutine(DashCoroutine());
    }

    void MovePlayer(float? speedOverride = null)
    {
        if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            float targetSpeed = speedOverride ?? speed;
            rb.velocity = new Vector3(moveDir.x * targetSpeed, rb.velocity.y, moveDir.z * targetSpeed);
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

        if (scaleCounter >= 35)
        {
            //lets just stop at this size for now...
            Renderer PlaneRenderer = GameObject.Find("Plane").GetComponent<Renderer>();
            PlaneRenderer.material = SlimeMaterial;

            //we have won
            mygamemanager.instance.EndGame();
            return;
        }

        if (scaleCounter >= 16 && scaleCounter < 28)
        {
            burstValueUpwards += 2f;
            speed += 5f;
        }

        if (scaleCounter >= 28)
        {
            burstValueUpwards += 8f;
            speed += 15f;
        }


        string collidedObjectName = "";
        collidedObjectName = collision.gameObject.name;
        //Debug.Log("We collided with "+collision.gameObject.name);

        if (collidedObjectName.Contains("Plane"))
        {
            return;
        }
        else
        {
            score +=  10;
        }

        // Change the material of the collided object to the slime one
        Renderer collidedRenderer = collision.gameObject.GetComponent<Renderer>();
        if (collidedRenderer != null)
        {
            collidedRenderer.material = SlimeMaterial;
        }

        // Calculate the new scale
        Vector3 newScale = gameObject.transform.localScale * (1f + scaleIncrease);
        
        // Apply the new scale
        transform.localScale = newScale;

        scaleCounter++;

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


        if (scaleCounter >= 12 && scaleCounter <= 25)
        {
            // Increase the radius of the TopRig orbit
            freeLookCamera.m_Orbits[0].m_Radius += 30f;

            // Increase the radius of the MiddleRig orbit
            freeLookCamera.m_Orbits[1].m_Radius += 30f;

            // Increase the radius of the BottomRig orbit
            freeLookCamera.m_Orbits[2].m_Radius += 30f;
            return;
        }
        
        groundDistance = 4f;
        // Increase the radius of the TopRig orbit
        freeLookCamera.m_Orbits[0].m_Radius += 500f;

        // Increase the radius of the MiddleRig orbit
        freeLookCamera.m_Orbits[1].m_Radius += 500f;

        // Increase the radius of the BottomRig orbit
        freeLookCamera.m_Orbits[2].m_Radius += 500f;
    }


    //method to check if we are a certain distance from the ground.
    private bool CheckGroundDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheckCenter.position, Vector3.down, out hit))
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

    //debug method
    private void CheckGroundDistance2()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheckCenter.position, Vector3.down, out hit))
        {
            float distanceToGround = hit.distance;
            //Debug.Log(distanceToGround);
        }
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;

        float elapsedTime = 0f;
        Vector3 initialVelocity = rb.velocity; // Store the current velocity

        // Optionally, you can clear the existing velocity if you want a more controlled dash
        rb.velocity = Vector3.zero;

        while (elapsedTime < dashDuration)
        {
            elapsedTime += Time.fixedDeltaTime;

            // Apply a portion of the dash force each frame
            rb.AddForce(transform.forward * (dashSpeed / dashDuration) * Time.fixedDeltaTime, ForceMode.VelocityChange);

            // Wait for the next FixedUpdate
            yield return new WaitForFixedUpdate();
        }

        // Optionally, you can restore the initial velocity or apply additional logic here
        isDashing = false;

        Invoke(nameof(DashCooldownHandler), dashCooldown);
    }

    private void DashCooldownHandler()
    {
        canDash = true;
    }
}
