using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTraitScript :EntityBaseScript, IEnemy
{
  
    public int damage = 1;
    
       public int GetDamage()
    {
       return damage;
    }

    public float GetKB()
    {
      return knockBack;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("FriendlyAttack"))
        {
            TakeDamage(1);//TODO change damage
        }
    }

  
    public void TakeDamage(int v)
    {
        if((health -= v) < 1)
        {
            die();
        }
    }

 
    // Start is called before the first frame update
}

  
