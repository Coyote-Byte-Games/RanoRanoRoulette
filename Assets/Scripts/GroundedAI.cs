using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedAI : MonoBehaviour
{
    public GameObject target;

    public int speed;

    #region Components
        private Rigidbody2D rb;
        private SpriteRenderer renderer;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        renderer = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    void TakeDamage(int v)
    {
        Destroy(gameObject);
    }
    public int GetDamage()
    {
       return 1;
    }

    void Update()
    {
        //if the enemy is far enough from the target
       this.rb.position += new Vector2(0 ,1E-4f);
        if (Mathf.Abs(rb.position.x - target.transform.position.x) < 25)
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
            break;
        }
        rb.velocity += Vector2.right * force;
        
    }} 
}
