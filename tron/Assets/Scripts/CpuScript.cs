using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpuScript : MonoBehaviour
{
    private Vector2 direction;
    //public float turnTime = 0.5f;
    private float turnTimer;
    
    public float speed = 3f;
    public GameObject wallPrefab;

    private Collider2D wall;

    private Vector2 lastWallEnd;
    int newDirection;
    private Collider2D aiCollider;
    public float linecastDistance = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        aiCollider = GetComponent<Collider2D>();
        direction = Vector2.left;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        newDirection = 4;
        spawnWall();
        SetRandomDirection();
        turnTimer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(direction * speed * Time.deltaTime);

        // Decrease the turn timer
        turnTimer -= Time.deltaTime;

        // Check for obstacles or decide to turn after some time
        if (turnTimer <= 0)
        {
            TurnRandomly();
            
        }
        
        fitColliderBetween(wall, lastWallEnd, transform.position);
    }
    
    void SetRandomDirection()
    {
        // Choose a random direction (up, down, left, right)
        int randomDir;
        do
        {
             randomDir = Random.Range(1, 5);
            
        } while ((randomDir % 2) == (newDirection % 2));

            switch (randomDir)
            {
                case 1:
                    direction = Vector2.up;
                    break;
                case 2:
                    direction = Vector2.right;
                    break;
                case 3:
                    
                    direction = Vector2.down;
                    break;
                case 4:
                    direction = Vector2.left;
                    break;
            }
            newDirection = randomDir;
            print(newDirection.ToString());

            GetComponent<Rigidbody2D>().velocity =  direction * speed;
    }
    
    bool IsObstacleAhead()
    {
        Vector2 start = transform.position;
        Vector2 sightStart = start + direction * 1f;
        Vector2 end = start + direction * linecastDistance;
        // Raycast in the current direction to check for obstacles
        RaycastHit2D hit = Physics2D.Linecast(sightStart, end );
        if (hit.collider)
        {
            // If the ray hits something, return true (obstacle ahead)
            print("Obstacle Ahead");
            return true;
        }
        return false;
    }
    
    void TurnRandomly()
    {
        // Choose a new random direction
        SetRandomDirection();

        // Reset the turn timer
        turnTimer = Random.Range(0.8f, 2.2f);
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
            print("Player Won");
            Destroy(gameObject);
        }
    }
}
