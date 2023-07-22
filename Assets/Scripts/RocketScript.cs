using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    public FreezeBehaviour freezeBehaviour;
    public EntityBaseScript entityBase;

    [Space]
    [HideInInspector]
    public Transform target;
    [Space]
    public float timeout = 5;
    public int speed;
    private Rigidbody2D rb;

    private SpriteRenderer render;
    private TrailRenderer trail;
    private float trailTempTime;
    private float timeLeft;


    public void Freeze()
    {
        freezeBehaviour.frozen = true;
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
    public void UnFreeze()
    {
        trail.time = trailTempTime;

    }

    // Start is called before the first frame update

    private void Explode()
    {
        // Destroy(gameObject);

        entityBase.die();
    }
    void Start()
    {
        freezeBehaviour.OnFreeze += Freeze;
        freezeBehaviour.OnUnfreeze += UnFreeze;
        trail = GetComponentInChildren<TrailRenderer>();
        timeLeft = timeout;
        // traits.AS = FindFirstObjectByType<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        render = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!freezeBehaviour.frozen)
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
            Explode();


    }
}
