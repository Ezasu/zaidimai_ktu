using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransition1 : MonoBehaviour
{
    public AudioSource oldMusic;
    public AudioSource newMusic;

    bool inTransition;
    bool musicChanged;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inTransition)
        {
            if (oldMusic.volume - Time.deltaTime < 0)
                oldMusic.volume = 0;
            else
                oldMusic.volume -= Time.deltaTime;

            if (newMusic.volume + Time.deltaTime > 1)
            {
                newMusic.volume = 1;
                inTransition = false;
            }
            else
                newMusic.volume += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !musicChanged)
        {
            inTransition = true;
            musicChanged = true;
            newMusic.volume = 0;
            newMusic.Play();
            newMusic.priority = oldMusic.priority + 1;
        }
    }
}
