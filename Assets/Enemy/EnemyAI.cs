using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    private int currentWayPoint = 0;
    private bool reachedEndPath = false;

    private Path path = null;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;


    [SerializeField] private LayerMask WhatIsWall;
    private bool walled;
    private bool grounded;

    [SerializeField] private float health = 10;

    public GameObject lootPrefab;
    public Item item;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;
        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndPath = true;
            return;
        }
        else
        {
            reachedEndPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        float force = Time.fixedDeltaTime * direction.x;

        walled = false;
        grounded = false;

        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, WhatIsWall);
        RaycastHit2D hitleft = Physics2D.Raycast(transform.position, Vector2.left, 0.8f, WhatIsWall);
        RaycastHit2D hitright = Physics2D.Raycast(transform.position, Vector2.right, 0.8f, WhatIsWall);
        if (groundCheck.collider != null)
        {
            grounded = true;
        }
        if (hitleft.collider != null || hitright.collider != null)
        {
            walled = true;
        }
        Vector3 targetVelocity = new Vector2(force * 300f, rb.velocity.y);
        if (walled && grounded)
            rb.AddForce(new Vector2(0f, 4800));
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.2f);



        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }
    }

    void OnDrawGizmos()
    {
        // Draw the ray in the Scene view for visualization purposes
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.left * 0.8f);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.right * 0.8f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down * 0.6f);
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(0f, 60f), ForceMode2D.Impulse);
        if (health < 0)
        {
            GameObject tmp = Instantiate(lootPrefab, transform.position, Quaternion.identity);
            tmp.GetComponent<Loot>().Initialize(item);
            Destroy(gameObject);
        }
    }
}

