﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Range(0, 15)]
    public float BouncingDelta = 0.05f;

    [Range(0, 20)]
    public float MaximumBounce = 1f;

    public bool NotMoving;

    public AudioClip sound;
    public AudioSource source;

    float currentPos;
    float startPos;


    int bounceDirection = 1;
    private void Awake()
    {
        currentPos = gameObject.transform.position.y;
        startPos = currentPos;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!NotMoving)
        {
            gameObject.transform.Rotate(0, 0, 1);
            currentPos += BouncingDelta * bounceDirection;
            if (Mathf.Abs(startPos - currentPos) > MaximumBounce)
            {
                bounceDirection *= -1;
            }
        }
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, currentPos, gameObject.transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            source.PlayOneShot(sound, 1f);
            currentPos = -100;
            Invoke("RemoveObject", 10);
        }

    }
    private void RemoveObject()
    {
        Destroy(gameObject);
    }
}
