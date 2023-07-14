using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class FollowerAI : MonoBehaviour
{
   
    public Transform target;
    public float speed = 200;
    public string targetTag;

     //placeholder
    public float nextWaypointDistance;//threshold to ge tthere

    Path path;
    public LayerMask groundLayer;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    public bool enemy;
    private int jumpsRemaining;
    Seeker seeker;
    public float jumpForce;
    Rigidbody2D rb;
    public bool CanJump;
    public GameObject jumpDetectorL; //these are for checking if the Ai needs to jump
    public GameObject jumpDetectorR; //these are for checking if the Ai needs to jump
    public GameObject groundCheck;
    private void _JumpUpdate()
    {
        if (!Physics2D.OverlapCircle(jumpDetectorL.transform.position, .2f, groundLayer))
        {
            _Jump();
        }
    }
    private void _Jump()
    {
        if (jumpsRemaining <= 0)
        {
            return;            
        }
        rb.AddForce(Vector2.up * jumpForce);
        jumpsRemaining --;
    }
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        target =GameObject.FindWithTag(targetTag).transform;

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

    public void SetTarget(RanoScript player)
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
        if (Physics2D.OverlapCircle(groundCheck.transform.position, .2f, groundLayer))
        {
            jumpsRemaining = 1;
        }
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
