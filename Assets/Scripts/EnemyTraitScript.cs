using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTraitScript :MonoBehaviour, IEnemy
{
    public float knockBack;
    public int damage;
    public int health;
    public GameObject boom;

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

    private void die()
    {
       var kablooey = Instantiate(boom, transform.position, Quaternion.identity);
        Destroy(kablooey, .25f);
        Destroy(gameObject);
    }
    // Start is called before the first frame update
}

  
