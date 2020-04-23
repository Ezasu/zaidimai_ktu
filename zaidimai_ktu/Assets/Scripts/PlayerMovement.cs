﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public class Buffer
    {
        public float[] Bufferis;
        int Current;

        public Buffer(int size)
        {
            Bufferis = (new float[size]).Select(e => (float)0).ToArray();
            Current = 0;
        }

        public void Push(float value)
        {
            Bufferis[Current] = value;
            Current = Current >= Bufferis.Length - 1 ? 0 : Current + 1;
        }
    }

    Rigidbody2D rb;
    public Animator anim;

    Vector2 beganTouch;
    Vector2 lastTouch;
    Vector2 scrollSpeed;


    public string playerScoreString = "Score: 0";
    int score;

    bool onGround, ascending, facingLeft, walking;

    Buffer lastVerticalVelocity = new Buffer(5);

    int directionX, directionY;

    [Range(0, 50)]
    public float MaxSpeed = 5f;

    [Range(0, 25)]
    public float SpeedMultiplier = 0.5f;

    [Range(0, 1)]
    public float slowDownMultiplier = 0.05f;

    [Range(0, 10)]
    public float animationRequiredWalkingSpeed = 0.7f;




    public float groundCheckRate = 0.0125f;

    public LayerMask groundLayer;

    [Range(0, 10)]
    public float groundHeight = 0.5f;

    public float heightOffset = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        score = 0;

        //InvokeRepeating("GroundCheck", 0, groundCheckRate);
    }

    private void OnGUI()
    {
        playerScoreString = GUI.TextField(new Rect(10, 10, 200, 40), $"Score: {score}", 25);
    }

    void Update()
    {
        GroundCheck();

        anim.SetBool("FacingLeft", facingLeft);
        anim.SetBool("JumpingUp", ascending);
        anim.SetBool("OnGround", onGround);
        anim.SetBool("Walking", walking);
        //if (Input.touchCount > 0)
        //{
        //    if (Input.GetTouch(0).deltaPosition.x >= 0)
        //        anim.SetBool("FacingLeft", true);
        //    else
        //        anim.SetBool("FacingLeft", false);
        //}

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            rb.AddForce(touch.deltaPosition * -1 * SpeedMultiplier);

            if (touch.deltaPosition.x != 0)
                facingLeft = touch.deltaPosition.x > 0;
            //if (touch.deltaPosition.x > 0)
        }
        else if (Input.touchCount == 0)
        {
            rb.velocity *= 1 - (slowDownMultiplier);
        }


        ascending = rb.velocity.y > 0;


        lastVerticalVelocity.Push(rb.velocity.y);

        if (lastVerticalVelocity.Bufferis.All(e => e == 0))
            onGround = false;

        walking = Mathf.Abs(rb.velocity.x) >= animationRequiredWalkingSpeed;



        //if (!onGround)
        //{
        //    anim.SetBool("JumpingUp", rb.velocity.y > 0);

            //}


            //return;
            //if (Input.touchCount == 1)
            //{
            //    var touch = Input.GetTouch(0);
            //    float newScrollSpeedX = touch.deltaPosition.x / touch.deltaTime * SpeedMultiplier * (-1);
            //    float newScrollSpeedY = touch.deltaPosition.y / touch.deltaTime * SpeedMultiplier * (-1);

            //    if (newScrollSpeedX != 0)
            //    //if (Mathf.Abs(newScrollSpeedX) > MinimumScrollSpeedToMovePlayer)
            //    {
            //        var newDirection = (newScrollSpeedX > 0 ? 1 : -1);
            //        if (newDirection != directionX && Mathf.Abs(rb.velocity.y) > 1) 
            //        { // jei scroll kryptis pakeista ir player juda (velocity > 1)
            //            scrollSpeedX *= 1.5f;
            //        }
            //        else
            //        {
            //            directionX = newDirection;//newScrollSpeedX > 0 ? 1 : -1;
            //            if (Mathf.Abs(scrollSpeedX + newScrollSpeedX) > MaxSpeed)
            //                scrollSpeedX = MaxSpeed * directionX;
            //            else
            //                scrollSpeedX = newScrollSpeedX;
            //        }

            //    }
            //    else
            //        scrollSpeedX = 0;
            //    if (Mathf.Abs(newScrollSpeedY) > MinimumScrollSpeedToMovePlayer)
            //    {
            //        directionY = newScrollSpeedY > 0 ? 1 : -1;
            //        if (Mathf.Abs(scrollSpeedY + newScrollSpeedY) > MaxSpeed)
            //            scrollSpeedY = MaxSpeed * directionY;
            //        else
            //            scrollSpeedY = newScrollSpeedY; 
            //    }
            //    else
            //        scrollSpeedY = 0;
            //    rb.AddForce(new Vector2(scrollSpeedX * 10, scrollSpeedY * 17));
            //}
            ////if (Mathf.Abs(rb.velocity.x) > MaxSpeed)
            ////{
            ////    rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * MaxSpeed, rb.velocity.y);
            ////}

            //if (transform.position.y < -2)
            //    transform.position = new Vector3(transform.position.x, 0, transform.position.z);

    }

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Level")
    //    {
            
    //        anim.SetBool("IsAirbone", false);
    //    }
    //}

    void GroundCheck()
    {
        //var ray = Physics2D.Raycast(transform.position, Vector2.down, DistanceToGround + 1.5f);
        //if (ray.collider is null || ray.collider.tag != levelTag)
        //    return false;
        //return true;
        bool grounded;
        if (Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z), Vector2.down, groundHeight + heightOffset, groundLayer))
            grounded = true;
        else
            grounded = false;

        if (grounded != onGround) // if onGround changed
        {
            //anim.SetBool("OnGround", grounded);
            //if (!grounded)
            //    anim.SetBool("JumpingUp", true);
            onGround = grounded;
        }

        else // if onGround didn't change
        {
            
        }
    }

    //void GroundCheck()
    //{
    //    var grounded = IsGrounded("Level");
    //    if (onGround != grounded)
    //    {
    //        anim.SetBool("OnGround", grounded);
    //        anim.SetBool("JumpingUp", !grounded);
    //        onGround = grounded;
    //    }
    //    else if (!grounded && rb.velocity.y <= 0)
    //    {
    //        anim.SetBool("JumpingUp", false);
    //    }
            
    //}

    void ChangeState(bool landed)
    {
        
    }


    //public void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Level")
    //    {
    //        anim.SetBool("IsAirbone", true);
    //    }
    //}

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "PowerUp")
            score++;
    }
}
