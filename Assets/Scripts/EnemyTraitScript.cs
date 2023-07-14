using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTraitScript :EntityBaseScript
{
    [Header("Enemy Traits")]
  public int aggroRange;
    
   
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if(other.collider.CompareTag("FriendlyAttack"))
        {
            Debug.Log("Enemy took damage");

            TakeDamage(1);//TODO change damage
        }
    }

  
   

 
    // Start is called before the first frame update
}

  
