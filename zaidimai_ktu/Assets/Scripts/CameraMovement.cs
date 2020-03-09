using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour
{
    Vector3 realPos;
    Vector3 tempPos;
    public GameObject Player;
    float yPosition; //kameros y pozicija
    float fingerPressedY; // y koordinate, kur pirstas prisiliete prie ekrano

    float strenght = 4.7f;

    [Range(0, 20)]
    public float playerSize = 20f;

    [Range(0, 0.5f)]
    public float returnSpeed = 0.4f;

    [Range(0, 1)]
    public float getSpeed = 0.5f;

    float touchBeganAt = 0;
    float camBeganAt = 0;
    float scrolledY = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 laikinas = Player.transform.position;
        laikinas.x = laikinas.x + 1;
        laikinas.y = laikinas.y;
        RaycastHit2D hit = Physics2D.Raycast(laikinas, Vector3.down);
        float distance = Player.transform.position.y - hit.point.y - 1.8348f;
        //Debug.Log(hit.point.y);
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchBeganAt = touch.position.y;
                camBeganAt = gameObject.transform.position.y;
            }
            scrolledY = touch.position.y - touchBeganAt - playerSize + gameObject.transform.position.y / 2;
            scrolledY = scrolledY / 100 * strenght; // kameros keliavimo greitis
            scrolledY = scrolledY - scrolledY / 2; // - camBeganAt
            Debug.Log(camBeganAt);
        }
        else
        {
            scrolledY = (float)(scrolledY - scrolledY * returnSpeed);
            touchBeganAt = 0;
        }
        camBeganAt = gameObject.transform.position.y * 0.8f;
        camBeganAt = 0;
        gameObject.transform.position = new Vector3(Player.transform.position.x, scrolledY + hit.point.y + camBeganAt, -10);
        //Debug.Log(gameObject.transform.position.y);
    }
}
