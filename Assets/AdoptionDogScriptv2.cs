using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class AdoptionDogScriptv2 : MonoBehaviour
{
    

    [Header("Pathfinding")]
    public GapJumpInitiative2024AllRighsReservedScript jumpGapScript;
    public LayerMask groundLayer;
    public Transform target;
    public float activateDistance = 50;
    public float pathUpdateInterval = .8f;
    [Header("Physics")]
    public float speed = 200;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirements = .8f;
    public float jumpModifier = .3f;
    public float jumpRaycastBottomOffset = .1f;
    public float jumpRaycastTopOffset = .1f;
    [Header("Custom Behaviour")]
    public bool followEnabled = true;
    public bool YDifferenceJumpEnabled = true;
    public bool GapJumpEnabled = true;
    public bool directionLook = true;
    //--------------------------------------------------------------------------\\
    private Path path;

    private int currentWaypoint = 0;
    bool isGrounded = false;
    private Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //To make the path actually work
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateInterval);
       
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position + Vector3.up * jumpRaycastTopOffset, Vector2.down * (GetComponent<Collider2D>().bounds.extents.y + jumpRaycastBottomOffset), Color.red);
    }
    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }
    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }
    private void PathFollow()
    {
        if (path is null)
        {
            return;
        }
        //reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }


        //Here we get the direction
        int polarity = rb.position.x < target.position.x ? 1 : -1;
        // Vector2 force = direction * speed * Time.deltaTime;
        //If the character can jump, is on the ground, and the direction justifies jumping, jump
        if (CanJump() && ShouldJump())// 
        {
            // rb.AddForce(Vector2.up * jumpModifier * Time.deltaTime, ForceMode2D.Impulse);
            rb.velocity = rb.velocity.x * Vector2.right + Vector2.up * jumpModifier;
        }
        //Adding movement force
        rb.position += (Vector2.right * polarity * speed * Time.deltaTime);
        //Checking if we need to progress thanks to being close enough or if we redo this movement
        var distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (directionLook)
        {
            if (polarity > 0)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponentInChildren<SpriteRenderer>().flipX = true;

            }
        }

    }

    private bool CanJump()
    {

        return YDifferenceJumpEnabled && Physics2D.Raycast(transform.position + Vector3.up * jumpRaycastTopOffset, Vector2.down, GetComponent<Collider2D>().bounds.extents.y + jumpRaycastBottomOffset, groundLayer);
    }

    private bool ShouldJump()
    {
        var direction = (((Vector2)path.vectorPath[currentWaypoint] - rb.position)).normalized;
        return 
        direction.y > jumpNodeHeightRequirements
        ||
        jumpGapScript.ShouldJump();
    }
    private bool TargetInDistance()
    {
        return Vector2.Distance(rb.position, target.position) < activateDistance;
    }
    //Waypoints are nodes in a path
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

}


//Personal Notes:
/*
*   -Headers seperating attributes and fields into "genres" makes for much clener code and organization
*   -Custom behaviour is defined by boolean variables for clean definition. if this was to be applied to a player character, we may add fields such as "Uses modifiers" and "Fancy Spawns" 
*   -Use functions for everything you can! The goal is to make code readable like english
*   -For pathfinding, use the vectorpath nodes and their attributes (y,x, etc) to determine important parts
*/