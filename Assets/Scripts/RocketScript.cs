using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : FreezableMonoBehaviour
{
    private Rigidbody2D rb;
    public SpriteRenderer render;
    public Transform target;
     private TrailRenderer trail;
    private float trailTempTime;

    public float timeout = 5;
    public EnemyTraitScript traits;
    public int speed;
    public PrimitiveAirAI ai;
    private float timeLeft;

    public override void Freeze()
    { base.Freeze();
        try
        {
        trail.time = Mathf.Infinity;
        trailTempTime = trail.time;
        }
        catch (System.Exception)
        {
            
           //sometimes the thing destroys as this is being called, nothing to really do about it
        }
        Debug.Log("rocket fmb called");
    }
      public override void UnFreeze()
    {
        base.UnFreeze();

        trail.time = trailTempTime;
       
    }

    // Start is called before the first frame update
   
    private void Explode()
    {
        // Destroy(gameObject);

        traits.die();
    }
    void Start()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        timeLeft = timeout;
        // traits.AS = FindFirstObjectByType<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        render = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
            
        if (!traits.frozen)
        {

            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                Explode();
            }
             //todo rotate
        Vector2 direction = ((Vector2)target.transform.position - rb.position).normalized;

        Vector2 force = direction * speed;

        rb.velocity = force;
      
        rb.SetRotation(Quaternion.LookRotation(direction));
        //simple flip x

        render.flipX = target.position.x < transform.position.x;

        }
       
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("rocket collided with " + other.gameObject.name);
        if (other.collider.CompareTag("Player"))
        {
            Explode();
        }
        

    }
}
