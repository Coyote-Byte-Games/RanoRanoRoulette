using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour, IEnemy
{
    public int GetDamage()
    {
       return 1;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("FriendlyAttack"))
        {
            TakeDamage(1);//TODO change damage
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int v)
    {
        Destroy(gameObject);
    }

    public float GetKB()
    {
      return 100;
    }

  
}
