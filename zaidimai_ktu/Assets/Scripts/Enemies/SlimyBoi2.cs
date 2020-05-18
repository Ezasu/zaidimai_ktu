using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlimyBoi2 : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    BoxCollider2D coll;
    Animator anim;
    SpriteRenderer spriteRenderer;

    [Range(0, 2)]
    public float WalkingSpeed = 0.5f;

    [Range(0, 1000)]
    public float VisibleDistance = 750f;

    [Range(0, 100)]
    public int Health = 100;

    public GameObject Player;
    public PlayerMovement PlayerScript;

    public bool AvoidFallingDown = true;

    Vector2 LeftEdge => new Vector2(coll.bounds.min.x - 1, coll.bounds.min.y);
    Vector2 RightEdge => new Vector2(coll.bounds.max.x + 1, coll.bounds.min.y);
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        if (Player is null || Player.ToString() == "null")
            Player = GameObject.Find("Player");
        if (PlayerScript is null || PlayerScript.ToString() == "null")
            PlayerScript = Player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Mathf.Abs(Player.transform.position.x - transform.position.x);//Vector2.Distance(Player.transform.position, transform.position);
        if (distanceToPlayer <= VisibleDistance && distanceToPlayer > 4.75f)
        {

            
        }
        else
        {
            if (distanceToPlayer < 4.5f && PlayerScript.onGround && (Mathf.Abs(Player.transform.position.y - transform.position.y) < 2f))
            {
                AttackPlayer();
            }
            anim.SetBool("Moving", false);
        }
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Mathf.Abs(Player.transform.position.x - transform.position.x);
        if (distanceToPlayer > VisibleDistance || distanceToPlayer < 4.25f)
            return;
        bool playerToTheRight = transform.position.x < Player.transform.position.x;

        if (playerToTheRight && !Physics2D.Raycast(RightEdge, Vector2.down) || !playerToTheRight && !Physics2D.Raycast(LeftEdge, Vector2.down))
        {
            anim.SetBool("Moving", false);
            return;
        }
        anim.SetBool("Moving", true);

        spriteRenderer.flipX = playerToTheRight;
        transform.position += new Vector3(WalkingSpeed * (playerToTheRight ? 1 : -1), 0);
    }

    void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            anim.SetBool("Hurt", false);
            anim.SetBool("Dying", true);
            Destroy(gameObject, 1f);
            PlayerScript.enemiesKilled++;
        }
    }

    void AttackPlayer()
    {
        var direction = Player.transform.position - transform.position;
        PlayerScript.TakeDamage(10, direction);
        anim.SetBool("Attacking", true);
        Invoke("NotAttacking", 1f);
        anim.SetBool("Hurt", false);
    }

    void NotAttacking()
    {
        anim.SetBool("Attacking", false);
    }

    void NotTakingDamage()
    {
        anim.SetBool("Hurt", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            //var collDirection = ((collision.GetContact(0).point - (Vector2)transform.position)).normalized;
            if (collision.GetContact(0).point.y > transform.position.y)
            {
                PlayerScript.DamagedEnemy();
                anim.SetBool("Hurt", true);
                TakeDamage(50);
                Invoke("NotTakingDamage", 1f);
                anim.SetBool("Attacking", false);
            }
            else 
            {
                AttackPlayer();
            }
        }
    }
}
