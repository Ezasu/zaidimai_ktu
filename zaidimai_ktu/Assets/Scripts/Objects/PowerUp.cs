using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Range(0, 15)]
    public float BouncingDelta = 0.05f;

    [Range(0, 20)]
    public float MaximumBounce = 1f;

    float currentPos;


    int bounceDirection = 1;
    // Start is called before the first frame update
    void Start()
    {
        currentPos = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Rotate(0, 0, 1);
        currentPos += BouncingDelta * bounceDirection;
        if (Mathf.Abs(currentPos) > MaximumBounce)
        {
            currentPos = MaximumBounce * bounceDirection;
            bounceDirection *= -1;
        }
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, currentPos, gameObject.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
            Destroy(gameObject);
    }
}
