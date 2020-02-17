using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Touch thisTouch;
    Touch lastTouch;
    float scrollSpeedX;

    [Range(0, 10)]
    public float MaxSpeed = 5f;

    [Range(0, 1)]
    public float SpeedMultiplier = 0.5f;

    bool slowingDown = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //Debug.ClearDeveloperConsole();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            scrollSpeedX = touch.deltaPosition.x / touch.deltaTime;
            if (touch.phase == TouchPhase.Began)
            {
                thisTouch = touch;
                lastTouch = touch;
                return;
            }
            thisTouch = touch;

            Debug.Log($"vel: {scrollSpeedX}");


            //rb.velocity = new Vector2(Mathf.Lerp(lastTouch.position.x, thisTouch.position.x, SpeedMultiplier), rb.velocity.y);

            rb.velocity = new Vector2(SpeedMultiplier * scrollSpeedX, rb.velocity.y);

            lastTouch = thisTouch;

            //if (touch.phase == TouchPhase.Ended)
            //{
            //    slowingDown = true;
            //}
            slowingDown = true;
        }
        else if (slowingDown)
        {
            rb.AddForce(new Vector2((-1) * rb.velocity.x / 200, 0f));
            //rb.velocity = new Vector2(rb.velocity.x / 2 , rb.velocity.y);
            if (Mathf.Abs(rb.velocity.x) < 0.01f)
            {
                slowingDown = false;
                Debug.ClearDeveloperConsole();
                Debug.Log("slowed down");
            }
        }

        //if (Input.touchCount > 0)
        //{
        //    thisTouch = Input.GetTouch(0);
        //    if (thisTouch.phase == TouchPhase.Began)
        //    {
        //        lastTouch = thisTouch;
        //    }
        //}

    }
}
