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
    private bool turned;
    public bool turns;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootPeriodically());

    }
    public void Update()
    {

        if (primAi.DoneScanning())
        {
            target = primAi.target;
        }

    }

    private IEnumerator ShootPeriodically()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(shootInterval);
            if (turns)
            {
                if ((transform.position - target.position).x < 0)
                {
                    turned = true;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    turned = false;
                    transform.localScale = new Vector3(1, 1, 1);

                }

            }
            if (Vector2.Distance(transform.position, target.position) < aggroRange && !freezeBehaviour.frozen)
            {
                StartCoroutine(Fire());

            }

        }
    }

    // Update is called once per frame

    private IEnumerator Fire()
    {

        rendererAnimatior.SetTrigger("Firing");
        yield return new WaitForSeconds(.3f);
        var rocket = Instantiate(rocketPrefab, firingPoint.position, Quaternion.identity);
        // rocket.SetActive(true);
// rocket.transform.localScale = transform.localScale;

        // rocket.GetComponent<EntityBaseScript>().AS =  FindFirstObjectByType<AudioSource>();
        rocket.GetComponent<RocketScript>().target = target;

        yield break;
    }
}
