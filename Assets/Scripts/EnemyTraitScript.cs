using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTraitScript : MonoBehaviour, IEnemy
{
    public float knockBack;
    public int damage;

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
            Debug.Log("ghost took damage");
        }
    }

    public void TakeDamage(int v)
    {
        Destroy(gameObject);
    }
    // Start is called before the first frame update
  }

  
