using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public Transform firework;

    [Range(0, 15)]
    public float BouncingDelta = 0.05f;

    [Range(0, 20)]
    public float MaximumBounce = 1f;

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
        firework.GetComponent<ParticleSystem>().enableEmission = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            currentPos = -100;
            firework.GetComponent<ParticleSystem>().enableEmission = true;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, currentPos, gameObject.transform.position.y);
            //Destroy(gameObject);
            source.Play();
            
            Invoke("RemoveObject", 10);
        }

    }
    private void RemoveObject()
    {
       
    }
}
