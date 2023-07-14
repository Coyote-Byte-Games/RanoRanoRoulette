using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusGunScript : FreezableMonoBehaviour
{
    [Header("Shooting Variables")]

    public float force;
    public float totalSpread = 20;
    public float shootInterval;
    public bool upsideDown = false;
    [Space]

    Rigidbody2D rb;
    public Transform anchorPoint;

    public GameObject projectile;

    public Transform firingPoint;
    public PrimitiveAirAI primAi;
    public EnemyTraitScript traits;
    [HideInInspector]
    public Animator rendererAnimatior;
    [HideInInspector]
    public SpriteRenderer renderer;
    [HideInInspector]
    public Transform target;
    private Vector2 firingPointDefault;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }
    public override void UnFreeze()
    {
        base.UnFreeze();
        frozen = false;
        rendererAnimatior.enabled = true;
    }
    void Start()
    {
        firingPointDefault =
        firingPoint.localPosition;




        StartCoroutine(ShootPeriodically());
        // rb = GetComponentInChildren<Rigidbody2D>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        rendererAnimatior = GetComponentInChildren<Animator>();
    }


    private IEnumerator ShootPeriodically()
    {
        //keep on shootin'
        for (; ; )
        {
            //keep checking if the ai is done scanning
            yield return new WaitForSeconds(.5f);
            if (primAi.DoneScanning())
            {
                for (; ; )
                {
                    yield return new WaitForSeconds(shootInterval);
                    if (!frozen && (Vector2.Distance(transform.position, target.position) < traits.aggroRange))
                    {
                        rendererAnimatior.SetBool("Firing", true);
                        float spread = UnityEngine.Random.Range(-(totalSpread / 2), totalSpread / 2);


                        Vector2 direction = ((target.position - transform.position)).normalized;
                        var rot = Quaternion.AngleAxis(spread, Vector3.forward) * direction;

                        // * ((target.position.x < transform.position.x) ? -1: 1)
                        var bullet = Instantiate(projectile, firingPoint.position, Quaternion.Euler(0, 0, rb.rotation + spread));
                        bullet.GetComponent<Rigidbody2D>().velocity = force * rot;
                    }
                    else
                    {
                        rendererAnimatior.SetBool("Firing", false);
                    }
                }
            }
            //after the interval, shoot the projectile.

        }
    }

    // Update is called once per frame
    void Update()
    {
        //flipping x if target is to the right

        if (primAi.DoneScanning())
        {
            if (!frozen)
            {
                target = primAi.target;
                Vector2 direction = -((Vector2)target.transform.position - rb.position).normalized;
                // Quaternion silly = Quaternion.LookRotation(direction) += 90; 
                // rb.SetRotation(Quaternion.LookRotation(direction));
                //clamped so we dont ruin everything
                rb.rotation =
                Mathf.Clamp(
                 (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90),
                 (upsideDown ? 110 : -70),
                 (upsideDown ? 250 : 70)
                 );
                // rb.rotation += 90;
                // rb.rotation = Mathf.Clamp(0 ,rb.rotation, 180);
                //    renderer.flipY = !(target.position.x < transform.position.x);
                //    firingPoint.localPosition = renderer.flipY? -firingPointDefault : firingPointDefault;  


            }


        }
    }

}
