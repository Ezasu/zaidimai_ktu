using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : MonoBehaviour
{
    [Range(0, 100)]
    public float Bounciness = 1.5f;

    public GameObject Player;
    private PlayerMovement playerScript;
    private Rigidbody2D playerRb;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = Player.GetComponent<PlayerMovement>();
        playerRb = Player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        //collision.collider.attachedRigidbody.AddForce(rb.velocity * -1 * Bounciness);
    }
}
