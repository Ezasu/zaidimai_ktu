using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    Vector2 beganTouch;
    Vector2 lastTouch;
    Vector2 scrollSpeed;


    public string playerScoreString = "Score: 0";
    int score;

    int directionX, directionY;

    [Range(0, 50)]
    public float MaxSpeed = 5f;

    [Range(0, 25)]
    public float SpeedMultiplier = 0.5f;

    [Range(0, 1)]
    public float slowDownMultiplier = 0.05f;

    [Range(0, 1000)]
    public float MinimumScrollSpeedToMovePlayer = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        score = 0;
    }

    private void OnGUI()
    {
        playerScoreString = GUI.TextField(new Rect(10, 10, 200, 40), $"Score: {score}", 25);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            rb.AddForce(touch.deltaPosition * -1 * SpeedMultiplier);
        }
        else if (Input.touchCount == 0)
        {
            rb.velocity *= 1 - (slowDownMultiplier);
        }
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

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "PowerUp")
            score++;
    }
}
