using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedAI : MonoBehaviour
{
    public GameObject target;

    public int speed;
    public bool CanJump;
    private int jumpsRemaining = 0;
    public float jumpForce;
    public LayerMask groundLayer;
    #region Components
    private Rigidbody2D rb;
    private SpriteRenderer renderer;
    public int aggroRange;
    public string seekTag;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        renderer = this.GetComponentInChildren<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        if (target)
        {
            return;
        }
        StartCoroutine(Scan());

    }
      private IEnumerator Scan()
    {
        yield return new WaitForSeconds(2f);
        target = FindObjectOfType<RanoScript>().gameObject;
        yield break;
    }

    public GameObject jumpDetectorL; //these are for checking if the Ai needs to jump
    public GameObject jumpDetectorR; //these are for checking if the Ai needs to jump
    public GameObject groundCheck;
    private void _JumpUpdate()
    {
        foreach (var detector in new GameObject[]{jumpDetectorL, jumpDetectorR})
        {
             if (!Physics2D.OverlapCircle(detector.transform.position, .2f, groundLayer))
        {
            _Jump();
        }
        }
       
    }
    private void _Jump()
    {
        if (jumpsRemaining <= 0)
        {
            return;
        }
        Debug.Log("doggi jump !!! wowww");
        rb.AddForce(Vector2.up * jumpForce);
        jumpsRemaining--;
    }

    public int GetDamage()
    {
        return 1;
    }

    void Update()
    {
        if (CanJump)
        {
            if (Physics2D.OverlapCircle(groundCheck.transform.position, .2f, groundLayer))
            {
                
                jumpsRemaining = 1;
            }
        _JumpUpdate();

        }

        //so it doesnt get caught
        this.rb.position += new Vector2(0, 1E-4f);
        //if the enemy is far enough from the target
      
        if (target && Mathf.Abs(rb.position.x - target.transform.position.x) < aggroRange)
        {





            var direction = ((target.transform.position.x - rb.position.x) * Vector2.right).normalized.x;

            var force = direction * speed * Time.deltaTime;
            switch (direction > 0)
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
            rb.velocity += Vector2.right * force;

        }
    }
}
