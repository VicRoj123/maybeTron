using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cpu2Script : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask layerMask;
    
    private Rigidbody2D rb;
    private Transform target;
    public GameObject playerTarget;
    public Vector2 movement;
    private List<Vector2> directions;
    
    public GameObject wallPrefab;

    private Collider2D wall;

    private Vector2 lastWallEnd;
    
                
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        target = GameObject.Find("You").transform;
        GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
        spawnWall();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirection();
        if (target)
        {
            
            print("hello");
            RaycastHit2D check = Physics2D.Raycast(transform.position, movement, 1f,layerMask);
            if (check.distance == 0)
            {
                GetComponent<Rigidbody2D>().velocity = movement * speed;
            }
            else
            {
                movement = -movement;
            }
            
            
        }
        
    }//End of update

    void ChangeDirection(){
        if (target.GetComponent<Rigidbody2D>().velocity == Vector2.up * speed ||
            target.GetComponent<Rigidbody2D>().velocity == Vector2.down * speed)
        {
            directions = new List<Vector2>{Vector2.right, Vector2.left};
        }
        else
        {
            directions = new List<Vector2>{Vector2.up, Vector2.down};
        }
        Vector2 firstDir = directions[0];
        var secondDir = directions[1];

        RaycastHit2D hitA = Physics2D.Raycast(transform.position, firstDir, 5f, layerMask);
        RaycastHit2D hitB = Physics2D.Raycast(transform.position, secondDir, 5f,layerMask);
        if (hitA.collider && hitB.collider)
        {
            if (hitA.distance >= hitB.distance)
            {
                
                movement = firstDir;
            }
            else if (hitA.distance < hitB.distance)
            {
                
                movement = secondDir;
            }
        }
        else if (hitA.collider)
        {
            
            movement = firstDir;
        }
        else if (hitB.collider)
        {
           
            movement = secondDir;
        }
        else
        {
           
            movement = firstDir;
        }
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
    

