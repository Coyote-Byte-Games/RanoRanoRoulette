using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FreezeBehaviour : MonoBehaviour
{
    public event Action OnFreeze;
    public event Action OnUnfreeze;
    internal bool frozen;
    internal Vector2 ownVelocity;
    internal RigidbodyConstraints2D ownConstraints;

 public void Start()
    {
        //In case the game is frozen
        if (TheWorldPlayerAction1.TheWorldActive)
        {
            Freeze();
        }
    }
    public void UnFreeze()
    {
        OnUnfreeze?.Invoke();
          //buncha try blocks to avoid nulls 
        try
        {
            var rb = GetComponentInChildren<Rigidbody2D>();
            frozen = false;
            rb.constraints = ownConstraints;
           

            rb.velocity = ownVelocity;
            
        }
        catch (System.Exception e)
        {
        }
        try
        {
             GetComponentInChildren<Animator>().enabled = true;
        }
        catch (System.Exception)
        {
            
            
        }


    }
    public void Freeze()
    {
        OnFreeze?.Invoke();
        frozen = true;
        //buncha try blocks to avoid nulls 
        try
        {
            var rb = GetComponentInChildren<Rigidbody2D>();
            ownConstraints = rb.constraints;
            ownVelocity = rb.velocity; 
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        catch (System.Exception)
        {

        }
        try
        {
            GetComponentInChildren<Animator>().enabled = false;
        }
        catch (System.Exception)
        {


        }


   
    }
    public void Awake()
    {
// rb = GetComponentInChildren<Rigidbody2D>();
    }

}


