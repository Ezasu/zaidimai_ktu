﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour
{
    Vector3 realPos;
    Vector3 tempPos;
    public GameObject Player;
    public Camera Camera;
    float yPosition; //kameros y pozicija
    float fingerPressedY; // y koordinate, kur pirstas prisiliete prie ekrano

    [Range(0f, 10f)]
    public float strenght = 1f;

    [Range(0f, 10f)]
    public float boostUp = 0f;

    [Range(0, 0.5f)]
    public float returnSpeed = 0.4f;

    [Range(0, 1)]
    public float getSpeed = 0.5f;

    float touchBeganAt = 0;
    float scrolledY = 0;
    float desiredPos = 0;
    float rayPos = 0;
    public bool ZoomEngaged = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OptionMove(int input)
    {
        boostUp = input;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ZoomEngaged && Camera.orthographicSize < 50)
        {
            Debug.Log("zoom");
            Camera.orthographicSize += 0.5f;
        }
        Vector3 laikinas = Player.transform.position;
        laikinas.x = laikinas.x + 1;
        RaycastHit2D hit = Physics2D.Raycast(laikinas, Vector3.down);
        if (hit.point.y != 0)
            rayPos = hit.point.y;
        float distance = Player.transform.position.y - rayPos;
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchBeganAt = touch.position.y;
            }
            scrolledY = touch.position.y - touchBeganAt;
            scrolledY = scrolledY / 100 * strenght; // kameros keliavimo greitis
            desiredPos = rayPos + scrolledY;
        }
        else
        {
            desiredPos = rayPos;
            touchBeganAt = 0;
        }
        var skirtumas = gameObject.transform.position.y - desiredPos;
        var pokytis = gameObject.transform.position.y - (skirtumas * 0.1f);
        gameObject.transform.position = new Vector3(Player.transform.position.x, pokytis + 0.5f + boostUp, -10);
    }
}
