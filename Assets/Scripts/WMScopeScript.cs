using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WMScopeScript : FreezableMonoBehaviour
{
    public Transform target;
    public Transform dummyHitbox;
    public AudioSource source;
    public WantedManModifier parent;
    public AudioClip creepySFX;
    public AudioClip kaboomSFX;
    public int speed;
    public int volume;

    private float deathTimer;
    private int deathtimerMax;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Behaviour());
        
    }
    public override void UnFreeze()
    {
        base.UnFreeze();
        frozen = false;
    }

    // Update is called once per frame
    void Update()
    {
       //movement
       //to give it some randomness
    
        Vector2 direction = (target.position - transform.position);
//+ Vector2.right*( transform.position.x % 5)

    transform.position += (Vector3)(direction.normalized * speed * direction.magnitude * Time.deltaTime *( frozen ? 0: 1 ));
    

        //Time based-Triggers
        

    }
    IEnumerator Behaviour()
    {
        //Give it x seconds to exist before taking action
        var one = 2f;
        yield return new WaitForSeconds(one);
        //Play eeiree sfx
      
        source.PlayOneShot(creepySFX, 1 );
      
        yield return new WaitForSeconds(4.5f-one);
        //fire
        while (frozen)
        {
           yield return new WaitForSeconds(.2f); 
        }
        Fire();
        Destroy(gameObject, 0.3f);
        yield break;

    }
    void Fire()
    {
        source.PlayOneShot(kaboomSFX );
      
      //lazy recoil
        Vector2 damageArea = transform.position;
        var hb = Instantiate(dummyHitbox, damageArea, Quaternion.identity);
        transform.position += Vector3.up * 5;

        
        //disable the damage in some way
        Destroy(hb.gameObject, .25f);
    }
}
