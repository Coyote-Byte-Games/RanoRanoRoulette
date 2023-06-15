using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObjectScript : MonoBehaviour
{
    public float knockBack;
    public int damage = 1;
    public Vector2 knockBackOverride;
    [SerializeField]
    public List<LayerMask> shitlist;
  
    public int GetDamage()
    {
       return damage;
    }

    public float GetKB()
    {
      return knockBack;
    }
    
}

  
