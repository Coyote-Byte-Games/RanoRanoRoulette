using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObjectScript : MonoBehaviour, IEnemy
{
    public float knockBack;
    public int damage = 1;

    public int GetDamage()
    {
       return damage;
    }

    public float GetKB()
    {
      return knockBack;
    }
}

  
