using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    float scrollSpeedX, scrollSpeedY;

    int directionX, directionY;

    [Range(0, 50)]
    public float MaxSpeed = 5f;

    [Range(0, 25)]
    public float SpeedMultiplier = 0.5f;

    [Range(0, 0.5f)]
    public float SlowDownMultiplier = 0.1f;

    [Range(0, 1000)]
    public float MinimumScrollSpeedToMovePlayer = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        scrollSpeedX = 0;
        scrollSpeedY = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            float newScrollSpeedX = touch.deltaPosition.x / touch.deltaTime * SpeedMultiplier * (-1);
            float newScrollSpeedY = touch.deltaPosition.y / touch.deltaTime * SpeedMultiplier * (-1);

            if (newScrollSpeedX != 0)
            //if (Mathf.Abs(newScrollSpeedX) > MinimumScrollSpeedToMovePlayer)
            {
                var newDirection = (newScrollSpeedX > 0 ? 1 : -1);
                if (newDirection != directionX && Mathf.Abs(rb.velocity.y) > 1) 
                { // jei scroll kryptis pakeista ir player juda (velocity > 1)
                    scrollSpeedX *= 1.5f;
                }
                else
                {
                    directionX = newDirection;//newScrollSpeedX > 0 ? 1 : -1;
                    if (Mathf.Abs(scrollSpeedX + newScrollSpeedX) > MaxSpeed)
                        scrollSpeedX = MaxSpeed * directionX;
                    else
                        scrollSpeedX = newScrollSpeedX;
                }

            }
            else
                scrollSpeedX = 0;
            //if (newScrollSpeedY != 0)
            if (Mathf.Abs(newScrollSpeedY) > MinimumScrollSpeedToMovePlayer)
            {
                //if ((newScrollSpeedY > 0 ? 1 : -1) != directionY)
                //{
                //    newScrollSpeedY *= 15f;
                //}
                directionY = newScrollSpeedY > 0 ? 1 : -1;
                if (Mathf.Abs(scrollSpeedY + newScrollSpeedY) > MaxSpeed)
                    scrollSpeedY = MaxSpeed * directionY;
                else
                    scrollSpeedY = newScrollSpeedY; 
            }
            else
                scrollSpeedY = 0;
            //if (scrollSpeedY != scrollSpeedX && scrollSpeedY != 0)
                rb.AddForce(new Vector2(scrollSpeedX * 10, scrollSpeedY * 15));
            //rb.MovePosition((Vector2)transform.position + new Vector2(scrollSpeedX, scrollSpeedY) * Time.deltaTime);

            //transform.Translate(new Vector3(scrollSpeedX, scrollSpeedY, 0) * Time.deltaTime);




            //rb.velocity = new Vector2(Mathf.Lerp(lastTouch.position.x, thisTouch.position.x, SpeedMultiplier), rb.velocity.y);


        }
        if (Mathf.Abs(rb.velocity.x) > MaxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * MaxSpeed, rb.velocity.y);
        }
    }
}
