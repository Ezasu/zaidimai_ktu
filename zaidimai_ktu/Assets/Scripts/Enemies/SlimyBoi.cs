using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimyBoi : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    BoxCollider2D coll;
    Animator anim;
    SpriteRenderer spriteRenderer;

    [Range(0, 20)]
    public float ShootingDelay = 0.075f;
    float shootingDelayCounter = 0;

    public float BeamLifeTime = 20f;

    public GameObject BeamTemplate;
    public GameObject Player;
    public PlayerMovement playerScript;

    [Range(0, 0.1f)]
    public float WalkingSpeed = 5f;

    [Range(0, 10)]
    public float ProjectileSpeed = 5f;

    [Range(0, 1000)]
    public float VisibleDistance = 500f;

    public bool Predicting = false;
    [Range(1, 10)]
    public float PredictionMagnitude = 1.12f;

    bool dead;

    Vector2 LeftEdge => new Vector2(coll.bounds.min.x - 1, coll.bounds.min.y);
    Vector2 RightEdge => new Vector2(coll.bounds.max.x + 1, coll.bounds.min.y);
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        if (Player is null || Player.ToString() == "null")
        {
            Player = GameObject.Find("Player");
        }
        if (BeamTemplate is null || BeamTemplate.ToString() == "null")
            BeamTemplate = GameObject.Find("Enemy_beam");// transform.GetChild(0).gameObject;
        coll = GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(coll, BeamTemplate.GetComponent<BoxCollider2D>());
        playerScript = Player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
        //if (Vector3.Distance(/*Player.transform.position*/target, transform.position) > VisibleDistance)
        if (distanceToPlayer > VisibleDistance)
        {
            anim.SetBool("Moving", false);
            return;
        }
        //anim.SetBool("Moving", true);
                

        if (shootingDelayCounter >= ShootingDelay && !dead)
        {
            shootingDelayCounter = 0;  
            Vector3 target = (Predicting) ? Player.transform.position + (playerScript.ApproxTrajectory())/*.normalized*/ * PredictionMagnitude : Player.transform.position;

            Vector3 dir = (/*Player.transform.position*/target - transform.position).normalized;
            GameObject newBeam = Instantiate(BeamTemplate);
            newBeam.transform.position = transform.position;
            float angle = transform.position.y > /*Player.transform.position*/target.y ? Vector2.Angle(Vector2.right, dir) : Vector2.Angle(Vector2.left, dir);
            newBeam.transform.Rotate(0, 0, angle * -1);
            //newBeam.transform.parent = gameObject.transform;
            newBeam.GetComponent<Beam>().Shoot(dir, ProjectileSpeed);

            Destroy(newBeam, BeamLifeTime);
        }
        else
            shootingDelayCounter += Time.deltaTime;


    }

    private void FixedUpdate()
    {
        bool playerToTheRight = transform.position.x < Player.transform.position.x;

        if (playerToTheRight && !Physics2D.Raycast(RightEdge, Vector2.down) || !playerToTheRight && !Physics2D.Raycast(LeftEdge, Vector2.down))
        {
            anim.SetBool("Moving", false);
            return;
        }
        anim.SetBool("Moving", true);

        spriteRenderer.flipX = playerToTheRight;
        //rb.velocity += new Vector2(WalkingSpeed * (playerToTheRight ? -1 : 1), 0);
        transform.position += new Vector3(WalkingSpeed * (playerToTheRight ? 1 : -1), 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            playerScript.DamagedEnemy();
            dead = true;
            anim.SetBool("Dying", true);
            Destroy(gameObject, 1f);
        }
            
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Level" && coll.name == "Death_Teleport")
        {
            Destroy(gameObject);
        }
    }
}
