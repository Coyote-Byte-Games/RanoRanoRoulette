using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : FreezableMonoBehaviour
{
    public int speed;
    private TrailRenderer trail;
    private float trailTempTime;
    public override void Freeze()
    {
        base.Freeze();
        try
        {
        GetComponent<FruityFlyScript>().frozen = true;
        trail.time = Mathf.Infinity;
        trailTempTime = trail.time;
        }
        catch (System.Exception)
        {
            
           //sometimes the thing destroys as this is being called, nothing to really do about it
        }
    }
    public override void UnFreeze()
    {
        base.UnFreeze();
        GetComponent<FruityFlyScript>().frozen =false;

        trail.time = trailTempTime;
       
    }
    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        // GetComponent<Rigidbody2D>().velocity = speed * transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 6)
        {
            var rb =GetComponent<Rigidbody2D>();
            //direction towards the thingy
            var direction = (other.collider.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;
            //add 90 degrees for perpendiular
            var perp =Quaternion.AngleAxis(90, Vector3.forward) * direction;//todo optimize
            //reflect with this new perpe
            Vector3 newVel = Vector3.Reflect(rb.velocity, perp);
            rb.velocity = newVel;
        }
        Destroy(gameObject);
    }
}
