using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RigMove : MonoBehaviour
{
    //Game Variables
    private Rigidbody rb;
    private Animator animator;
    public Camera Camera;
    public LayerMask ground;
    public Transform groundCheck;
    private AudioSource source;

    //Game Arrays and Data
    private float baseFOV;
    private float sprintFOVModifier = 1.5f;
    public float speed = 12f;
    public float jumpForce = 100f;
    public float sprintModifier;
    public int Health = 100;
    public bool jump;
    public bool isJumping;
    public AudioClip JumpSound;
    public AudioClip[] WalkSounds;
    public AudioClip LandSound;


    void Start()
    {
        source = GetComponent<AudioSource>();
        baseFOV = Camera.fieldOfView;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //Controller Inputs 
        float x = Input.GetAxis("Horizontal") + SimpleInput.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical") + SimpleInput.GetAxis("Vertical");

        //States
        jump = Input.GetKey(KeyCode.Space);
        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        //State Logic.
        bool isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.5f, ground);
        isJumping = jump && isGrounded;
        bool isSprinting = sprint && z > 0 && !isJumping && isGrounded;

        //Movement Logic
        Vector3 t_direction = new Vector3(x,0,z);
        t_direction.Normalize();

        //Jumping
        if (isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce); animator.SetBool("IsJumping", true);
            animator.SetBool("IsIdle", false);
            PlayJumpSound();
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }


    /// sprinting
    float t_adjustSpeed = speed;
        if (isSprinting) t_adjustSpeed *= sprintModifier;
        if (isSprinting) { Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView,baseFOV * sprintFOVModifier,Time.deltaTime * 8f); }
        else { Camera.fieldOfView = baseFOV; }

         //Moving
         Vector3 t_targetVelocity = transform.TransformDirection(t_direction) * t_adjustSpeed * Time.deltaTime;
        t_targetVelocity.y = rb.velocity.y;
        rb.velocity = t_targetVelocity;
        
        if (rb.velocity.y < -2 && isGrounded)
        {
            PlayLandSound();
        }

    }

    void Update()
    {

            //animations
            if (Input.GetAxisRaw("Vertical") != 0 || SimpleInput.GetAxis("Vertical") != 0)
            {
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsIdle", false);
            }
            else if (Input.GetAxisRaw("Vertical") == 0 || SimpleInput.GetAxis("Vertical") == 0)
            {
                animator.SetBool("IsWalking", false);
            }
            if (Input.GetAxisRaw("Horizontal") != 0 || SimpleInput.GetAxis("Horizontal") != 0)
            {
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsIdle", false);
            }

            //else
            //{
            //   animator.SetBool("IsWaling", false);
            //}

            if (Health <= 0)
            {
                animator.enabled = false;
                this.enabled = false;
                GetComponent<Shoot>().enabled = false;

            }
    }
    

   void PlayWalkSound()
   {
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, WalkSounds.Length);
        source.clip = WalkSounds[n];
        source.PlayOneShot(source.clip);
        // move picked sound to index 0 so it's not picked next time
        WalkSounds[n] = WalkSounds[0];
        WalkSounds[0] = source.clip;
   }

   void PlayJumpSound()
   {
       source.clip =JumpSound;
       source.Play();
   }
        
   void PlayLandSound()
   {
     source.clip = LandSound;
     source.Play();
   }
    

}
