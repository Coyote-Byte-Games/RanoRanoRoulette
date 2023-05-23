using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBaseScript : MonoBehaviour
{
    public float knockBack;
    public int health = 1;
    public GameObject boom;
      public AudioSource AS;
    public AudioClip[] SFX;

    internal void die()
    {
       var kablooey = Instantiate(boom, transform.position, Quaternion.identity);
         AS.PlayOneShot(SFX[0]); //kaboom

        Destroy(kablooey, .25f);
        Destroy(gameObject);
    }
    // Start is called before the first frame update
}

  
