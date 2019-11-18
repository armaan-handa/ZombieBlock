using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public float JumpHeight = 6f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Animator animator;
    public CharacterController characterController;
    public bool running;
    public bool jumping;
    Vector3 velocity;
    float animSpeedPercent;
    float animHor;
    float animVer;
    bool isGrounded;
    // Update is called once per frame

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        // check if player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -12f;
            animator.SetBool("isJumping", false);
            jumping = false;
        }

        // get inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 inputDir = transform.right * x + transform.forward * z;
        inputDir = inputDir.normalized;

        // select whether to run or walk
        running = (Input.GetButton("Aim") ? false : (Input.GetKey(KeyCode.LeftShift) && (x != 0 || z != 0)));
        float speed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
  
        characterController.Move(inputDir * speed * Time.deltaTime);
        
        velocity.y += gravity * Time.deltaTime;
        
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(JumpHeight);
            animator.SetBool("isJumping", true);
            jumping = true;

        }
        
        characterController.Move(velocity * Time.deltaTime); // add vertical movement
        
        // play walk/run animation
        animSpeedPercent = (running ? 1 : 0.5f) * inputDir.magnitude;  
        
        // animation direction
        animHor = (z == -1 ? -x : x);
        animVer = z;

        // set animation parameters
        animator.SetFloat("SpeedPercent", animSpeedPercent);
        animator.SetFloat("Right", animHor);
        animator.SetFloat("Forward", animVer);
        
    }
}
