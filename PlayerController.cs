using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection { Left, Right }
    public float walkVelocity = 1f;
    public float runVelocity = 2.5f;
    public float slowDownVelocity = 0.8f;
    public float lowJumpVelocity = 2.5f;
    public float highJumpVelocity = 4f;
    public float currentXVelocity;
    public float currentYVelocity;

    //public Vector3 moveDirection;
    //public float maxDashTime = 1.0f;
    //public float dashSpeed = 2.0f;
    //public float dashStoppingSpeed = 0.1f;

    private float currentDashTime;

    FacingDirection facingDirection;
    public bool grounded;
    public bool walking;
    public bool running;
    public bool doubleJumped;

    Rigidbody rb;
    Ray Ray;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //currentDashTime = maxDashTime;
    }

        private void Update()
    {
        MoveCharacter();
        //data for debug
        currentXVelocity = Mathf.Abs(rb.velocity.x);
        currentYVelocity = Mathf.Abs(rb.velocity.y);
        
        rb.AddRelativeForce(1 * walkVelocity * Time.deltaTime * 10 * Input.GetAxisRaw("Horizontal"), -lowJumpVelocity * Time.deltaTime * 40, 0, ForceMode.Impulse);
    }
    
    private void FixedUpdate()
    {
//this is gravity//
        rb.AddRelativeForce(0, -lowJumpVelocity * Time.deltaTime * 60, 0, ForceMode.Impulse);

    }
    private void MoveCharacter()
    {

//i tried clamping down the jump vel//
        if (rb.velocity.y > 10) {  Mathf.Clamp(rb.velocity.y ,- 10,10); }
        
//jump & double jump//
        if (grounded )
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddRelativeForce(1 * walkVelocity * Time.deltaTime * 70 * Input.GetAxisRaw("Horizontal"), highJumpVelocity * Time.deltaTime * 60, 0, ForceMode.Impulse);
            }
        }
        if (!grounded&& !doubleJumped)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddRelativeForce(1 * walkVelocity * Time.deltaTime * 70 * Input.GetAxisRaw("Horizontal"), highJumpVelocity * Time.deltaTime * 60, 0, ForceMode.Impulse);
                doubleJumped = true;
            }

        }
//dash that doesn't work
        //if (Input.GetButtonDown("Horizontal"))
        //{
        //    currentDashTime = 0.0f;
        //}

        //if (currentDashTime < maxDashTime)
        //{
        //    moveDirection = new Vector3(0, 0, dashSpeed);
        //    currentDashTime += dashStoppingSpeed;
        //}
        //else
        //{
        //    moveDirection = Vector3.zero;
        //}
        //Controller.move(moveDirection * Time.deltaTime);
    
    //air movement//
        if (!grounded && rb.velocity.y <= 0)
        {
            rb.AddRelativeForce(Vector3.up * walkVelocity * Time.deltaTime * 60 * Input.GetAxisRaw("Horizontal"), ForceMode.Acceleration);
        }

//fall movement//
        if ((!grounded && rb.velocity.y <= 0)|| Input.GetButtonUp("Jump"))
        {
            rb.AddRelativeForce(1 * walkVelocity * Time.deltaTime * 20 * Input.GetAxisRaw("Horizontal"), -lowJumpVelocity * Time.deltaTime * 20, 0, ForceMode.Impulse);
            Debug.Log("down");
            Debug.Log("1");
        }

//air movement
        if (!grounded && Input.GetButton("Horizontal"))
        {

            if (currentXVelocity < walkVelocity)
            {
                rb.AddRelativeForce(Vector3.right * runVelocity * Time.deltaTime * 50 * Input.GetAxisRaw("Horizontal"), ForceMode.Impulse);
            }

        }


//walking & running//
//acceleration//
        if (grounded&&Input.GetButton("Horizontal"))
        {
            rb.AddRelativeForce(Vector3.right * runVelocity * Time.deltaTime * 40 * Input.GetAxisRaw("Horizontal"), ForceMode.Impulse);
            walking = true;
        }
//deceleration//
        if (grounded && Input.GetButtonUp("Horizontal"))
        {
            rb.AddRelativeForce(Vector3.right * walkVelocity * Time.deltaTime * 50 * -Input.GetAxisRaw("Horizontal"), ForceMode.Impulse);
        }
//for animation//
        if (currentXVelocity > 2.5|| currentYVelocity > 0.1)
        {
            running = true;
        }

//maintaining run speed 
        if (grounded && running)
        {
            if (currentXVelocity<7.5)
            {
                rb.AddRelativeForce(Vector3.right * runVelocity * Time.deltaTime * 60 * Input.GetAxisRaw("Horizontal"), ForceMode.Acceleration);
                Debug.Log("Running");
            }
        }
//deceleration & animation//
        if (Input.GetButtonUp("Horizontal"))
        {
            rb.AddRelativeForce(Vector3.right * slowDownVelocity * Time.deltaTime* 10 * -Input.GetAxisRaw("Horizontal"), ForceMode.VelocityChange);
            walking = false;
            running = false;
        }
    }
    
/////////////
//colliders///
    private void OnCollisionStay(Collision collision)
    {
        if (collision == null)
        {
            grounded = false;
        }
        if ( collision.gameObject.layer == 8)
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision == null)
        {
            grounded = false;
        }
        if (collision.gameObject.layer == 8)
        {
            grounded = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == 8)
        {
            grounded = true;
            doubleJumped = false;
        }
    }
    public bool IsWalking()
    {
        return walking;
    }

    public bool IsGrounded()
    {
        return grounded;

    }

////For animation/////
    public FacingDirection GetFacingDirection()
    {
        if (rb.velocity.x < 0) { facingDirection = FacingDirection.Left; }
        if (rb.velocity.x > 0) { facingDirection = FacingDirection.Right; }
        return facingDirection;
    }



}
