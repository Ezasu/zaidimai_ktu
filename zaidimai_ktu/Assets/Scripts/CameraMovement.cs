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
    public float strenght = 10;

    [Range(0, 1)]
    public float returnSpeed = 0.1f;

    [Range(0, 1)]
    public float getSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                var test = Camera.main.ScreenToWorldPoint(touch.position).y;
                
                fingerPressedY = Player.transform.position.y - Camera.main.ScreenToWorldPoint(touch.position).y;
            }
            float pressedPos;
            if (touch.position.y > Screen.height / 2) // virsus
                pressedPos = (Screen.height / 2 - touch.position.y) / Screen.height * strenght - (touch.position.y / Screen.height * strenght) / 2;
            else                                   // apacia
                pressedPos = (Screen.height / 2 - touch.position.y) / Screen.height * strenght - (touch.position.y / Screen.height * strenght) / 2;
            float skirtumas = pressedPos - yPosition;
            //Debug.Log("Skirtumas = " + skirtumas);
            yPosition = (float)(yPosition + skirtumas * getSpeed) * -1;
            yPosition += fingerPressedY * 0.8f;
            //if (skirtumas > 0 && Mathf.Abs(pressedPos) > Mathf.Abs(yPosition))
            //    yPosition = (float)(yPosition - pressedPos * 0.3);
            //if (skirtumas < 0 && Mathf.Abs(pressedPos) > Mathf.Abs(yPosition))
            //{
            //    //yPosition = (float)(yPosition - pressedPos * 0.3);
            //}
            gameObject.transform.position = new Vector3(Player.transform.position.x, yPosition, -10); // ATIMTI POZICIJA KURIA GAUSI IS RAY
            //Debug.Log("Max pozicija : " + Screen.height + "Paliesta pozicija : " + touch.position.y / Screen.height);
        }
        else if (Mathf.Abs(gameObject.transform.position.y - Player.transform.position.y) > 0.02)
        {
            float skirtumas = gameObject.transform.position.y - Player.transform.position.y;
            if (skirtumas > 0)
                yPosition = (float)(yPosition - yPosition * returnSpeed);
            if (skirtumas < 0)
            {
                yPosition = (float)(yPosition - yPosition * returnSpeed);
            }

            gameObject.transform.position = new Vector3(Player.transform.position.x, yPosition, -10);
        }
        else
        {
            yPosition = Player.transform.position.x;
            gameObject.transform.position = new Vector3(Player.transform.position.x, yPosition, -10);
        }
        //Debug.Log(Player.transform.position.y);
        Vector3 laikinas = Player.transform.position;
        laikinas.x = laikinas.x+1;
        laikinas.y = laikinas.y;
        RaycastHit2D hit = Physics2D.Raycast(laikinas, Vector3.down);
        float distance = Player.transform.position.y - hit.point.y - 1.8348f;
        //Ray ray = new Ray(Player.transform.position, Vector3.down);
        Debug.Log(distance);
    }
}
