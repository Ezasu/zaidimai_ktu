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

    float strenght = 5f;

    [Range(0, 20)]
    public float playerSize = 20f;

    [Range(0, 0.5f)]
    public float returnSpeed = 0.4f;

    [Range(0, 1)]
    public float getSpeed = 0.5f;

    float touchBeganAt = 0;
    float camBeganAt = 0;
    float scrolledY = 0;
    float desiredPos = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 laikinas = Player.transform.position;
        laikinas.x = laikinas.x + 1;
        RaycastHit2D hit = Physics2D.Raycast(laikinas, Vector3.down);
        float distance = Player.transform.position.y - hit.point.y;
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchBeganAt = touch.position.y;
            }
            scrolledY = touch.position.y - touchBeganAt;
            scrolledY = scrolledY / 100 * strenght; // kameros keliavimo greitis
            desiredPos = hit.point.y + scrolledY;
        }
        else
        {
            desiredPos = hit.point.y;
            touchBeganAt = 0;
        }
        var skirtumas = gameObject.transform.position.y - desiredPos;
        var pokytis = gameObject.transform.position.y - (skirtumas * 0.1f);
        gameObject.transform.position = new Vector3(Player.transform.position.x, pokytis, - 10);
    }
}
