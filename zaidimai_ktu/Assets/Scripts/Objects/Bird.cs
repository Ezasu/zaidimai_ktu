using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public GameObject Player;

    [Range(0, 2)]
    public float Speed = 11.5f;

    [Range(0, 300)]
    public float SpawnDistance = 50f;

    [Range(0, 200)]
    public float DisappearDistance = 50f;

    public bool GoingLeft;

    private bool seenPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (GoingLeft)
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(Speed * (GoingLeft ? -1 : 1), 0, 0);

        if (seenPlayer)
        {
            if (Vector2.Distance(Player.transform.position, transform.position) > DisappearDistance)
            {
                seenPlayer = false;
                transform.position = new Vector3(Player.transform.position.x - (SpawnDistance * (GoingLeft ? -1 : 1)), transform.position.y, 0);
            }
        }
        else
        {
            if (Vector2.Distance(Player.transform.position, transform.position) < SpawnDistance / 3)
                seenPlayer = true;
        }


    }
}
