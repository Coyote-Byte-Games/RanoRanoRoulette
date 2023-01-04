using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class bettertestplayablescript : MonoBehaviour{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public float jumpRadius;
    List<Modifier> mods = new List<Modifier>();
    BoxCollider2D bc;
    [SerializeField]
    public Transform groundCheck;
    public int speed;
    public LayerMask groundLayer;
    public int jumpPower;
    bool grounded;

//why interfaces
   
    void CheckForModConflicts()
    {
        //getting a list of repeated interfaces
        var dupes = mods.SelectMany(mod => mod.GetType().GetInterfaces(), (mod, interfaces) => new {mod, interfaces} ). //using the mods, create objects for each interface per object.
        GroupBy(i => i.interfaces). //defines a grouping based on the interfaces that the mod has
        Where(g => g.Count() > 1). //g is each grouping, defined by the prior statement. g is not an object, but a reference to a tracker of objects. We're checking if there are more than a single instance of each interface.
        Select(g => g.Key); //returns the duplicate interfaces

        foreach (Type Imod in dupes)
        {
            //run code to handle each conflicting mod
            switch (Imod)
            {
                //Then something just snapped, something inside of me. “No! No more! That’s it! I don’t care!” I didn’t care anymore. 
                //I didn't care about "modularity," or "Solid Principles."
                //no seriously screw this engine im coding in assembly seeya team 
                case IJumpModifier:
                    //just gotta see what exactly we need to deal with here
                break;
                case IMovementModifier:

                break;
                default:
                throw new NotImplementedException("whoooooooooooOOOOOOPS we did [not] add in functionality for that !!!! ! ! ! ! ! Please contact HR at femboygaming2002@gmail.com");
            }
        }
        
    }
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
            mod.OnStartEffect(this);
            StartCoroutine(mod.ContinuousEffect(this));
            //todo end effect
            
       

    }
    bool Grounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, jumpRadius, groundLayer);
    }
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
















