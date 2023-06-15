using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezableMonoBehaviour : MonoBehaviour
{
    internal bool frozen;
    private RigidbodyConstraints2D ownConstraints;

    public virtual void UnFreeze()
    {
        try
        {
            frozen = true;
            GetComponent<Rigidbody2D>().constraints = ownConstraints;
            GetComponent<Animator>().enabled = true;
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
            var rb = GetComponent<Rigidbody2D>();
            ownConstraints = rb.constraints;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        catch (System.Exception)
        {

        }
        try
        {
            GetComponent<Animator>().enabled = false;

        }
        catch (System.Exception)
        {


        }



    }

}


