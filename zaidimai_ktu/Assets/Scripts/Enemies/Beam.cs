using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    // Start is called before the first frame update
    //Rigidbody2D rb;
    public Vector3 direction = Vector3.zero;
    public float FiringSpeed = 100;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (direction != Vector3.zero)
        {
            transform.position += direction * FiringSpeed;
        }
    }

    public void Shoot(Vector3 direction, float speed)
    {
        this.FiringSpeed = speed / 10;
        GetComponent<SpriteRenderer>().enabled = true;
        this.direction = direction;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
