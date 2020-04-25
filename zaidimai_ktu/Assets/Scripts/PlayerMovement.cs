using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public class Buffer
    {
        public Vector2[] Bufferis;
        int Current;

        public Buffer(int size)
        {
            Bufferis = (new Vector2[size]).Select(e => Vector2.zero).ToArray();//(new float[size]).Select(e => 0f).ToArray();
            Current = 0;
        }

        public void Push(Vector2 value)
        {
            Bufferis[Current] = value;
            Current = Current >= Bufferis.Length - 1 ? 0 : Current + 1;
        }

        public Vector2 LastValue(int index = 0)
        {
            if (index > Bufferis.Length)
                index = 0;
            var _index = Current - 1 - index < 0 ? Bufferis.Length - 1 - index : Current - 1 - index;
            return Bufferis[_index];
        }
    }

    Rigidbody2D rb;
    public Animator anim;

    Vector2 beganTouch;
    Vector2 lastTouch;
    Vector2 scrollSpeed;


    public string playerScoreString = "Score: 0";
    int score;

    bool onGround, ascending, facingLeft, walking,
        onSlipperyPlatform;

    Buffer lastVelocity = new Buffer(5);

    int directionX, directionY;

    [Range(0, 50)]
    public float MaxSpeed = 5f;

    [Range(0, 25)]
    public float SpeedMultiplier = 0.5f;

    [Range(0, 1)]
    public float slowDownMultiplier = 0.05f;

    [Range(0, 10)]
    public float animationRequiredWalkingSpeed = 0.7f;

    [Range(0, 25)]
    public float BouncyPlatformBounciness = 3.5f;

    [Range(0, 500)]
    public float SlipperyPlatformSlipperiness = 200f;



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
            var slowDown = slowDownMultiplier / (onSlipperyPlatform ? SlipperyPlatformSlipperiness : 1);
            rb.velocity *= 1 - (slowDown);
        }


        ascending = rb.velocity.y > 0;


        lastVelocity.Push(rb.velocity);

        if (lastVelocity.Bufferis.Select(e => e.y).All(e => e == 0))
            onGround = false;

        walking = Mathf.Abs(rb.velocity.x) >= animationRequiredWalkingSpeed;


        {
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
    }

    //private void OnCollisionEnter2D(Collision2D coll)
    //{
    //    switch (coll.gameObject.tag)
    //    {
    //        case "Level_bouncy":
    //            rb.velocity = new Vector2(rb.velocity.x, lastVelocity.LastValue() * -2);
    //            break;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.collider.gameObject;
        if (obj.tag == "Level")
        {
            if (obj.name.StartsWith("Slippery"))
            {
                onSlipperyPlatform = true;
            }
            else if (obj.name.StartsWith("Bouncy"))
            {
                rb.velocity = lastVelocity.LastValue() * -1 * BouncyPlatformBounciness;
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        var obj = collision.collider.gameObject;
        if (obj.tag == "Level")
        {
            if (obj.name.StartsWith("Slippery"))
            {
                onSlipperyPlatform = false;
            }
        }
    }



    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "PowerUp")
            score++;
        else if (coll.tag == "Level" && coll.name == "Death_Teleport")
            transform.position = Vector3.zero;

    }
}
