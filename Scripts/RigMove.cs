using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigMove : MonoBehaviour
{
    //Declaring objects to be defined later
    private Rigidbody rb;
    public LayerMask ground;
    public Camera Camera;
    public Transform groundCheck;
    
    //Game Arrays and Data
    private float baseFOV = 60f;
    private float sprintFOVModifier = 1.5f;
    public float speed = 12f;
    public float jumpForce = 100f;
    public float sprintModifier;
    public bool jump;
    public bool isJumping;

    void Start()
    {
        // getting the rigidbody component from the object
        rb = GetComponent<Rigidbody>();
    }

 void FixedUpdate()
    {
        //Controller Inputs from Pre-defined axis
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Defining states 
        jump = Input.GetKey(KeyCode.Space);
        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        //Configuring State Logic
        bool isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.5f, ground);
        isJumping = jump && isGrounded;
        bool isSprinting = sprint && z > 0 && !isJumping && isGrounded;

        //Configuring Movement Logic
        Vector3 t_direction = new Vector3(x,0,z);
        t_direction.Normalize();

        //Jumping
        if (isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }

        float t_adjustSpeed = speed;
        if (isSprinting) t_adjustSpeed *= sprintModifier;
        if (isSprinting) { Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView,baseFOV * sprintFOVModifier,Time.deltaTime * 8f); }
        else { Camera.fieldOfView = baseFOV; }

        //Moving
        Vector3 t_targetVelocity = transform.TransformDirection(t_direction) * t_adjustSpeed * Time.deltaTime;
        t_targetVelocity.y = rb.velocity.y;
        rb.velocity = t_targetVelocity;
    }
}
