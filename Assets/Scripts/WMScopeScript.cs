using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WMScopeScript : MonoBehaviour
{
    public FreezeBehaviour freezeBehaviour;
    public Transform target;
    public Transform dummyHitbox;
    [HideInInspector]
    
    public AudioSource source;
    [HideInInspector]
    public WantedManModifier parent;
    public AudioClip creepySFX;
    public AudioClip kaboomSFX;
    public int speed;

    void Start()
    {
        StartCoroutine(Behaviour());

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (target.position - transform.position);
        //move if not frozen
        if (freezeBehaviour.frozen)
        {
            return;
        }
        else
        {
            transform.position += (Vector3)(direction.normalized * (speed * direction.magnitude * Time.deltaTime));
        }
    }
    IEnumerator Behaviour()
    {
        //Give it x seconds to exist before taking action
        var one = 2f;
        yield return new WaitForSeconds(one);
        //Play eeiree sfx

        source.PlayOneShot(creepySFX, 1);

        yield return new WaitForSeconds(4.5f - one);
        //fire
        while (freezeBehaviour.frozen)
        {
            yield return new WaitForSeconds(.2f);
        }
        Fire();
        Destroy(gameObject, 0.3f);
        yield break;

    }
    void Fire()
    {
        source.PlayOneShot(kaboomSFX);

        //lazy recoil
        Vector2 damageArea = transform.position;
        var hb = Instantiate(dummyHitbox, damageArea, Quaternion.identity);
        transform.position += Vector3.up * 5;


        //disable the damage in some way
        Destroy(hb.gameObject, .25f);
    }
}
