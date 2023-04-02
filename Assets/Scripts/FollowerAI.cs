using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class FollowerAI : MonoBehaviour
{
   
    public Transform target;
    public float speed = 200;

     //placeholder
    public float nextWaypointDistance;//threshold to ge tthere

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    public bool enemy;
    Seeker seeker;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        

        InvokeRepeating(nameof(UpdatePath), 0f, .5f);

    }
     void TakeDamage(int v)
    {
        Destroy(gameObject);
    }
    public int GetDamage()
    {
       return 1;
    }

    public void SetTarget(bettertestplayablescript player)
    {
        target = player.transform;
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            // float playerDir = Mathf.Sign(target.position.x - rb.position.x);
            seeker.StartPath(rb.position, new Vector2( target.position.x, target.position.y-1), OnPathComplete);

        }
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        else
        {
            Debug.Log("path killed itself");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (path is null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        Vector2 direction = (((Vector2)path.vectorPath[currentWaypoint] - rb.position)).normalized ;
        //TODO Create better code for navigating in a straight line, jumping, and others
        Vector2 force = (direction * speed * Time.deltaTime);
        Debug.Log("force "+force.x);
        if (force.x == 0)
        {
            //force it to reconsider
            UpdatePath();
        }
        rb.velocity += (force);
          var renderer = GetComponent<SpriteRenderer>();

        switch (force.x > 0)
        {
            case true:
            renderer.flipX = false;
            break;
            case false:
            renderer.flipX = true;
            break;
            default: 
            //do not modify the turn if no input
          
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }


    }
}
