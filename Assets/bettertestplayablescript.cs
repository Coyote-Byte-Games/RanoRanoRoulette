using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bettertestplayablescript : MonoBehaviour{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public float jumpRadius;// = .2f;

    [SerializeField]
    //All of the effects applied every frame from modifiers
    // List<Action<bettertestplayablescript>> ContEffects = new List<Action<bettertestplayablescript>>();
     List<Modifier> mods = new List<Modifier>();
    BoxCollider2D bc;
    [SerializeField]
    public Transform groundCheck;
    public int speed;
    
    public LayerMask groundLayer;
    public int jumpPower;
     bool grounded;
//! DEPRECTAED, F O S S I L
    //  IEnumerator ApplyContEffects()
    //  {
    //     foreach (var item in ContEffects)
    //     {
    //         item.Invoke(this);
    //     }
    //     yield return new WaitForSeconds(.2f);
    //  }
    void Start()
    {
        jumpRadius=.2f;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        groundCheck = transform.GetChild(0).gameObject.transform;
       
    }

    public void AddModifier(Modifier mod)//! this may be broken, idk
    {

            this.mods.Add(mod);
            mod.OnStartEffect.Invoke(this);
            //to be constantly triggered. The cont effect, can, in theory say when it's done
            
            StartCoroutine(mod.ContinuousEffect.Invoke(this));
            //todo add the ending effect
            //mod.OnEndEffect.Invoke(this);
            
       

    }

    bool Grounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, jumpRadius, groundLayer);
    }
    // Update is called once per frame

    void Update()
    {
        //handle horizontal movement
        rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0f));

        if(Grounded() && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpPower * Time.deltaTime);
        }

        
    }
}
















