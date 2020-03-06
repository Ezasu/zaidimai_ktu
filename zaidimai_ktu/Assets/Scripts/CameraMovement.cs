using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector3 realPos;
    Vector3 tempPos;
    public GameObject Player;
    float yPosition; //kameros y pozicija

    float fingerPressedY; // y koordinate, kur pirstas prisiliete prie ekrano

    [Range(0,50)]
    public float strenght = 4.7f;

    [Range(0, 20)]
    public float playerSize = 20f;

    [Range(0, 0.5f)]
    public float returnSpeed = 0.4f;

    [Range(0, 1)]
    public float getSpeed = 0.5f;

    float touchBeganAt = 0;
    float scrolledY = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(Player.transform.position.y);
        Vector3 laikinas = Player.transform.position;
        laikinas.x = laikinas.x + 1;
        laikinas.y = laikinas.y;
        RaycastHit2D hit = Physics2D.Raycast(laikinas, Vector3.down);
        float distance = Player.transform.position.y - hit.point.y - 1.8348f;
        //Ray ray = new Ray(Player.transform.position, Vector3.down);
        //Debug.Log(distance);
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                //touchBeganAt = Camera.main.ScreenToWorldPoint(touch.position).y;
                touchBeganAt = touch.position.y;


                //fingerPressedY = Player.transform.position.y - Camera.main.ScreenToWorldPoint(touch.position).y;
                
            }
            scrolledY = touch.position.y - touchBeganAt - playerSize;
            scrolledY = scrolledY / 100 * strenght;
            scrolledY = scrolledY - scrolledY / 2;
            Debug.Log(scrolledY);
            gameObject.transform.position = new Vector3(Player.transform.position.x, scrolledY, -10);
            //float pressedPos;
            //pressedPos = (Screen.height / 2 - touch.position.y) / Screen.height * strenght - (touch.position.y / Screen.height * strenght) / 2;
            //float skirtumas = pressedPos - yPosition;
            ////Debug.Log("Skirtumas = " + skirtumas);
            //yPosition = (float)(yPosition + skirtumas * getSpeed) * -1;
            ////yPosition = fingerPressedY;
            ////Debug.Log(yPosition);
            //if (skirtumas > 0 && Mathf.Abs(pressedPos) > Mathf.Abs(yPosition))
            //    yPosition = (float)(yPosition - pressedPos * 0.3);
            //if (skirtumas < 0 && Mathf.Abs(pressedPos) > Mathf.Abs(yPosition))
            //{
            //    //yPosition = (float)(yPosition - pressedPos * 0.3);
            //}
            //gameObject.transform.position = new Vector3(Player.transform.position.x, yPosition, -10); // ATIMTI POZICIJA KURIA GAUSI IS RAY
            //Debug.Log("Max pozicija : " + Screen.height + "Paliesta pozicija : " + touch.position.y / Screen.height);
        }
        else //if (Mathf.Abs(gameObject.transform.position.y - Player.transform.position.y) > 0.02)
        {
            float skirtumas = gameObject.transform.position.y - Player.transform.position.y;
            float skirtumas2 = scrolledY - Player.transform.position.y;
            //yPosition = (float)(yPosition - yPosition * returnSpeed);
            scrolledY = (float)(scrolledY - scrolledY * returnSpeed);
            touchBeganAt = 0;
            gameObject.transform.position = new Vector3(Player.transform.position.x, scrolledY, -10);
        }
        //else
        //{
        //    yPosition = Player.transform.position.x;
        //    gameObject.transform.position = new Vector3(Player.transform.position.x, yPosition, -10);
        //}
    }
}
