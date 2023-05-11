using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedAI : MonoBehaviour
{
    public GameObject boom;
    public GameObject target;

    public int speed;

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
    }


    public int GetDamage()
    {
       return 1;
    }

    void Update()
    { 
        //so it doesnt get caught
        this.rb.position += new Vector2(0 ,1E-4f);
        //if the enemy is far enough from the target
        if (Mathf.Abs(rb.position.x - target.transform.position.x) < aggroRange)
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
