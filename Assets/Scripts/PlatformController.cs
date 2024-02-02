using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformController : MonoBehaviour
{
    public Transform[] points;
    public int speed = 5;
    public float waitDuration = 0.5f;
    public bool isClosed = true;
    public bool spriteInFirstPoint = true;
    public float distanceToChange = 0.1f;

    private int currentPoint = 0;
    private Vector3 targetPos;

    private  Vector2 moveDirection;
    private Rigidbody2D rb;
    private PlayerController player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        targetPos = points[currentPoint].position;
        transform.position = points[currentPoint].position;
        Direction();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[currentPoint].position) < distanceToChange)
        {
            NextDirection();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }

    void NextDirection()
    {
        // If the platform is closed, it will go to the next point
        currentPoint++;
        if (isClosed)
        {
            if (currentPoint >= points.Length)
            {
                currentPoint = 0;
            }
        }
        // If the platform is not closed, it will go to the previous point
        else
        {
            if (currentPoint >= points.Length)
            {
                currentPoint = 0;
                System.Array.Reverse(points);
            }
        }
        moveDirection = Vector2.zero;
        targetPos = points[currentPoint].position;

        StartCoroutine(WaitNextPoint());
    }

    IEnumerator WaitNextPoint()
    {
        yield return new WaitForSeconds(waitDuration);
        Direction();
    }

    void Direction()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            Debug.Log("play Y " + player.transform.position.y + " platform Y " + transform.position.y);
            if (player && player.transform.position.y > transform.position.y)
            {
                player.IsOnPlatform = true;
                player.PlatformRb = rb;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (player)
            {
                player.IsOnPlatform = false;
                player.PlatformRb = null;
                player = null;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (spriteInFirstPoint)
        {
            transform.position = points[0].position;
        }
        else
        {
            transform.position = points[points.Length - 1].position;
        }


        // Draw a line between the points if not closed path
        if (!isClosed)
        {
            for (int i = 0; i < points.Length - 1; i++)
            {
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }
        }

        // Draw a line between the points if closed path
        else
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (i < points.Length - 1)
                {
                    Gizmos.DrawLine(points[i].position, points[i + 1].position);
                }
                else
                {
                    Gizmos.DrawLine(points[i].position, points[0].position);
                }
            }
        }

    }
}
