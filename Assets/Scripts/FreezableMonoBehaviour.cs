using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezableMonoBehaviour : MonoBehaviour
{
    internal bool frozen;
    internal Vector2 ownVelocity;
    internal RigidbodyConstraints2D ownConstraints;

    public virtual void UnFreeze()
    {
        try
        {
            var rb = GetComponentInChildren<Rigidbody2D>();
            frozen = false;
            rb.constraints = ownConstraints;
           

            rb.velocity = ownVelocity;
            
        }
        catch (System.Exception e)
        {
            Debug.Log("shat the bed " + e);
        }
        try
        {
             GetComponentInChildren<Animator>().enabled = true;
        }
        catch (System.Exception)
        {
            
            
        }


    }
    public virtual void Freeze()
    {
        frozen = true;

        //haha kms

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


