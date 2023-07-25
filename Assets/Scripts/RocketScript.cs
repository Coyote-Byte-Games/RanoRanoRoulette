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
    public float turnCrappiness = 0.3f;
    public float timeToAdjust = 0.3f;

    private SpriteRenderer render;
    private TrailRenderer trail;
    private float trailTempTime;
    private float timeLeft;
    private Vector2 optimalDirection = Vector2.left;
    private Vector2 trueDirection = Vector2.left;


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
    private float timer = 0;
    void Start()
    {
        trueDirection = optimalDirection;
        freezeBehaviour.OnFreeze += Freeze;
        freezeBehaviour.OnUnfreeze += UnFreeze;
        trail = GetComponentInChildren<TrailRenderer>();
        timeLeft = timeout;
        // traits.AS = FindFirstObjectByType<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        render = GetComponentInChildren<SpriteRenderer>();

        StartCoroutine(SlowRotate(trueDirection, optimalDirection, timeToAdjust));
        // rb.SetRotation(Quaternion.LookRotation((Vector2)target.transform.position - rb.position).normalized);
        //   rb.velocity = speed * ((Vector2)target.transform.position - rb.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {

// timer += Time.deltaTime;
// if (timer > turnCrappiness)
// {
//      StartCoroutine(SlowRotate(trueDirection, optimalDirection, turnCrappiness));
// }
        if (!freezeBehaviour.frozen)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                Explode();
            }
            
            //todo rotate
            optimalDirection = ((Vector2)target.transform.position - rb.position).normalized;


            Vector2 force = trueDirection * speed;

            rb.velocity = speed * trueDirection;

            rb.SetRotation(Quaternion.LookRotation(trueDirection));
            //simple flip x

            render.flipX = target.position.x < transform.position.x;

        }

    }
    //vectors are structs and thus value types.
    private IEnumerator SlowRotate(Vector3 input, Vector2 desired, float timeToAdjust)
    {
        for (; ; )
        {
            float t = 0;
            while (t < timeToAdjust)
            {
                trueDirection  = Vector3.Lerp(trueDirection, optimalDirection, t/turnCrappiness);
                t += Time.deltaTime;
                yield return null;
            }
        // timeToAdjust = this.timeToAdjust;

        }
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        Explode();


    }
}
