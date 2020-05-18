using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public class Buffer
    {
        public Vector2[] Bufferis;
        int Current;
        public bool IsFull => Bufferis.Any(e => e != Vector2.zero); 

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

    BoxCollider2D coll;
    Rigidbody2D rb;
    public Animator anim;

    Vector2 beganTouch;
    Vector2 lastTouch;
    Vector2 scrollSpeed;


    public string playerScoreString = "Score: 0";
    int score;
    public int enemiesKilled = 0;

    public bool onGround;
    bool ascending, facingLeft, walking,
        onSlipperyPlatform;

    Buffer lastVelocity = new Buffer(5);
    Buffer lastPositions = new Buffer(20);

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

    int HP 
    {
        get { return Health; }
        set 
        {
            if (value < 0)
                value = 0;
            Health = value; 
            UpdateHealth(Health); 
        }
    }

    [Range(0, 100)]
    public int Health = 100;

    public float groundCheckRate = 0.0125f;

    public LayerMask groundLayer;

    [Range(0, 10)]
    public float groundHeight = 0.5f;

    public float heightOffset = 0.25f;

    public Transform HealthBar;

    public TMPro.TMP_Text ingameScore;
    public TMPro.TMP_Text gameOverScore;
    public TMPro.TMP_Text display;


    Vector2 LeftEdge => new Vector2(coll.bounds.min.x - 1, coll.bounds.min.y);
    Vector2 RightEdge => new Vector2(coll.bounds.max.x + 1, coll.bounds.min.y);

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        score = 0;

        if (HealthBar is null || HealthBar.ToString() == "null")
            HealthBar = GameObject.Find("Bar").transform;
        //InvokeRepeating("GroundCheck", 0, groundCheckRate);

        Physics2D.IgnoreLayerCollision(9, 9);
    }
    float timer = 0.0f;
    int seconds;
    private void OnGUI()
    {
        if (HP != 0)
            display.text = "You won!";
        gameOverScore.text = $"Your score: {score}\n Time taken: {seconds}";
        ingameScore.text = $"Your score: {score}";
        //playerScoreString = GUI.TextField(new Rect(10, 60, 75, 40), $"Score: {score}", 25);
    }

    void UpdateHealth(int newHealth)
    {
        HealthBar.localScale = new Vector3(newHealth / 100f, 1f);
    }
    

    void Update()
    {
        GroundCheck();

        anim.SetBool("FacingLeft", facingLeft);
        anim.SetBool("JumpingUp", ascending);
        anim.SetBool("OnGround", onGround);
        anim.SetBool("Walking", walking);
        timer += Time.deltaTime;
        if (HP != 0)
            seconds = (int)(Math.Round(timer % 60, 1));
        if (HP == 0)
        {
            
            FindObjectOfType<MenuManager>().GameOver();
            gameObject.SetActive(false);
        }
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

        lastPositions.Push(rb.position);

        lastVelocity.Push(rb.velocity);

        //if (lastVelocity.Bufferis.Select(e => e.y).All(e => e == 0))
        //    onGround = false;

        walking = Mathf.Abs(rb.velocity.x) >= animationRequiredWalkingSpeed;
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
        if (Physics2D.Raycast(new Vector3(/*transform.position.x*/LeftEdge.x, /*transform.position.y*/LeftEdge.y + heightOffset, transform.position.z), Vector2.down, groundHeight + heightOffset, groundLayer)
            || Physics2D.Raycast(new Vector3(RightEdge.x, RightEdge.y + heightOffset, transform.position.z), Vector2.down, groundHeight + heightOffset, groundLayer))
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
        switch (obj.tag)
        {
            case "Level":
                if (obj.name.StartsWith("Slippery"))
                {
                    onSlipperyPlatform = true;
                }
                else if (obj.name.StartsWith("Bouncy"))
                {
                    rb.velocity = lastVelocity.LastValue() * -1 * BouncyPlatformBounciness;
                }
                break;

            case "Enemy":
                /*if (obj.name.Contains("SlimyBoi"))
                {
                    HP += 7;
                }
                else*/ if (obj.name.Contains("Beam"))
                {
                    HP -= 20;
                    rb.velocity -= ((collision.GetContact(0).point - (Vector2)transform.position)).normalized * 25f;
                } 
                   
                break;
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
        if (coll.tag == "Level" && coll.name == "Finish Flag")
            score += 100;
        if (coll.tag == "PowerUp")
            score++;
        else if (coll.tag == "Level" && coll.name == "Death_Teleport")
        {
            //transform.position = Vector3.zero;
            HP = 0;
            //FindObjectOfType<MenuManager>().GameOver();
        }
    }

    public void TakeDamage(int damage, Vector3? collisionDirection = null)
    {
        if (collisionDirection != null)
        {
            //rb.AddForce(((Vector2)collisionDirection).normalized * 250f);
            rb.AddForce(new Vector2(collisionDirection.Value.normalized.x, 0.5f) * 1500f);
        }
        HP -= damage;
    }

    public void DamagedEnemy()
    {
        rb.AddForce(Vector2.up * 800f);
        score++;
    }

    public Vector3 ApproxTrajectory()
    {
        //var debug = rb.position - lastPositions.LastValue();
        return rb.position - lastPositions.LastValue();
    }
}
