using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBox : MonoBehaviour
{
    private Rigidbody2D rb;

    [Range(0, 50)]
    public float PushForce = 7.5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Vector2 direction = coll.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
            rb.AddForce(direction.normalized * PushForce);
        }
    }
}
