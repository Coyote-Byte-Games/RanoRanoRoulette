using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBaseScript : MonoBehaviour
{
   
    public int health = 1;
    public GameObject boom;
      public AudioSource AS;
    public AudioClip[] SFX;

    public void Freeze()
    {
      GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }
    internal virtual void die()
    {

      try
      {
         var npcness = GetComponent<TalkativeNPC>();
      //cant talk if youre dead moron
      npcness.Shaddup();
      }
      catch (System.Exception)
      {
        //yeah im just gonna do nothing how do you like that bitch your "clean code" can go straight to hell
      }
     

       var kablooey = Instantiate(boom, transform.position, Quaternion.identity);
         AS.PlayOneShot(SFX[0]); //kaboom

        Destroy(kablooey, .25f);
        Destroy(gameObject);
    }
    
    public void Start()
    {
      if (AS is null)
      {
        AS = FindAnyObjectByType<AudioSource>();
      }
    }
    // Start is called before the first frame update
}

  
