using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLShooterScript : MonoBehaviour
{
    public FreezeBehaviour freezeBehaviour;
    public Transform target;
    public int aggroRange = 100;
    public float shootInterval;
    public Transform firingPoint;
    public Animator rendererAnimatior;
    public GameObject rocketPrefab;
    public PrimitiveAirAI primAi;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootPeriodically());
        
    }

    private IEnumerator ShootPeriodically()
    {
        for(;;)
        {
            yield return new WaitForSeconds(shootInterval);
            if (Vector2.Distance(transform.position, target.position) < aggroRange && !freezeBehaviour.frozen)
            {
            StartCoroutine(Fire());
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //flipping x if target is to the right
        
        if (primAi.DoneScanning())
        {
            target = primAi.target;
        }
    }
    private IEnumerator Fire()
    {
        rendererAnimatior.SetTrigger("Firing");
        yield return new WaitForSeconds(.3f);
        var rocket = Instantiate(rocketPrefab, firingPoint.position, Quaternion.identity );
        // rocket.GetComponent<EntityBaseScript>().AS =  FindFirstObjectByType<AudioSource>();
        rocket.GetComponent<RocketScript>().target = target;
        
        yield break;
    }
}
