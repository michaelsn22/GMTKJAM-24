using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    public float growthFactor = 1.1f; // 10% increase
    public float shrinkFactor = 0.9f; // 10% decrease

    public float scaleChange = 0.1f; // 10% change
    private Vector3 baseScale;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        baseScale = transform.localScale;
    }

    void Update()
    {
        Debug.Log(isGrounded);
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");

        // Calculate movement vector
        Vector3 move = transform.right * moveHorizontal;

        // Apply movement
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GrowCharacter();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ShrinkCharacter();
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement
        controller.Move(velocity * Time.deltaTime);
    }

    private void GrowCharacter()
    {
        AdjustCharacterScale(scaleChange);
    }

    private void ShrinkCharacter()
    {
        AdjustCharacterScale(-scaleChange);
    }

    private void AdjustCharacterScale(float scaleDelta)
    {
        // Calculate new scale
        Vector3 newScale = transform.localScale + baseScale * scaleDelta;

        // Calculate scale factor
        float scaleFactor = newScale.y / transform.localScale.y;

        // Apply new scale
        transform.localScale = newScale;

        // Adjust the CharacterController properties
        controller.height *= scaleFactor;
        controller.radius *= scaleFactor;

        // Adjust the center of the CharacterController
        float heightDifference = controller.height * (scaleFactor - 1);
        controller.center += new Vector3(0, heightDifference / 2f, 0);

        // Adjust the position of the ground check
        groundCheck.localPosition = new Vector3(0, -controller.height / 2f, 0);

        // Adjust other properties
        moveSpeed *= scaleFactor;
        jumpForce *= scaleFactor;

        // Ensure the character doesn't become too small
        if (transform.localScale.y < baseScale.y * 0.1f)
        {
            Debug.Log("Character has reached minimum size!");
            transform.localScale = baseScale * 0.1f;
        }

        Debug.Log(transform.localScale);
    }
}
