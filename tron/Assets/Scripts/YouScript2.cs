using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouScript2 : MonoBehaviour
{
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode rightKey;
    public KeyCode leftKey;
    
    public float speed = 5f;
    public GameObject wallPrefab;

    private Collider2D wall;

    private Vector2 lastWallEnd;
    // Use this for initialization
    void Start ()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
        spawnWall();
    }

    // Update is called once per frame
    void Update () 
    {
        if (Input.GetKeyDown(upKey)) {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
            spawnWall();
        }
        else if (Input.GetKeyDown(downKey)) {
            GetComponent<Rigidbody2D>().velocity = -Vector2.up * speed;
            spawnWall();
        }
        else if (Input.GetKeyDown(rightKey)) {
            GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
            spawnWall();
        }
        else if (Input.GetKeyDown(leftKey)) {
            GetComponent<Rigidbody2D>().velocity = -Vector2.right * speed;
            spawnWall();
        }    
        
        fitColliderBetween(wall, lastWallEnd, transform.position);
    }

    void spawnWall()
    {
        lastWallEnd = transform.position;
        GameObject g = Instantiate(wallPrefab, transform.position, Quaternion.identity);
        wall = g.GetComponent<Collider2D>();
    }

    void fitColliderBetween(Collider2D collider, Vector2 a, Vector2 b)
    {
        collider.transform.position = a + (b - a) * 0.5f;

        float dist = Vector2.Distance(a, b);
        if (a.x != b.x)
        {
            collider.transform.localScale = new Vector2(dist + 1, 1);
        }
        else
        {
            collider.transform.localScale = new Vector2(1, dist + 1);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != wall)
        {
            print("Player lost");
            Destroy(gameObject);
        }
    }
}
