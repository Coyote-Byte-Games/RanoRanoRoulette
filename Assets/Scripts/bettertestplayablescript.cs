using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class bettertestplayablescript : MonoBehaviour{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public GameData data;
    public float jumpRadius;
    List<IModifier> mods = new List<IModifier>();
    BoxCollider2D bc;
    [SerializeField]
    public Transform groundCheck;
    public int speed;
    public LayerMask groundLayer;
    public int jumpPower;
    bool grounded;
    private PlayerState playerState = new PlayerState();

//why interfaces
   
   //?no longer needed
    //// void CheckForModConflicts()
    //// {
    ////     //getting a list of repeated interfaces
    ////     var dupes = mods.SelectMany(mod => mod.GetType().GetInterfaces(), (mod, interfaces) => new {mod, interfaces} ). //using the mods, create objects for each interface per object.
    ////     GroupBy(i => i.interfaces). //defines a grouping based on the interfaces that the mod has
    ////     Where(g => g.Count() > 1). //g is each grouping, defined by the prior statement. g is not an object, but a reference to a tracker of objects. We're checking if there are more than a single instance of each interface.
    ////     Select(g => g.Key); //returns the duplicate interfaces
    ////     foreach (Type Imod in dupes)
    ////     {
    ////         //run code to handle each conflicting mod
    ////         switch (Imod)
    ////         {
    ////             //Then something just snapped, something inside of me. “No! No more! That’s it! I don’t care!” I didn’t care anymore. 
    ////             //I didn't care about "modularity," or "Solid Principles."
    ////             //no seriously screw this engine im coding in assembly seeya team 
    ////             case IJumpModifier:
    ////                 //just gotta see what exactly we need to deal with here
    ////             break;
    ////             case IMovementModifier:
    ////             break;
    ////             default:
    ////             throw new NotImplementedException("whoooooooooooOOOOOOPS we did [not] add in functionality for that !!!! ! ! ! ! ! Please contact HR at femboygaming2002@gmail.com");
    ////         }
    ////     }
        
    void SetCircleCollider()
    {
        gameObject.AddComponent<CircleCollider2D>();
        Destroy(GetComponent<BoxCollider2D>());
    }
    public Collider2D GetCollider()
    {
        return GetComponent<Collider2D>();
    }
    public float GetVel()
    {
        return rb.velocity.x;
    }
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        groundCheck = transform.GetChild(0).gameObject.transform;
       
    }
    public void AddAction(IPlayerAction action)
    {
        this.playerState.AddAction(action);
    }
    void Action()
    {
        //when the player clicks the action key, we launch the current action
        IPlayerAction action = playerState.GetAction();
        action.Run();
    }
    void ChangeAction()
    {
        playerState.ChangeAction();
    }
    public void AddModifier(IModifier mod)//! this may be broken, idk
    {

            this.mods.Add(mod);
            mod.SetPlayer(this);
            mod.OnStartEffect(this);
            mod.SetPlayerEffects(this);//yes i know this is terrible it smells like garbage but blame unity for no

            StartCoroutine(mod.ContinuousEffect(this));
            //todo end effect
            //find a way to set the player effects in the player script
       

    }
    public bool Grounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, jumpRadius, groundLayer);
    }
    void Update()
    {
        //handle horizontal movement
        rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0f));

        if(Grounded() && Input.GetButtonUp("Jump"))
        {
            rb.AddForce(Vector2.up * jumpPower * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Action();
            Debug.Log("used action");

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeAction();
            Debug.Log("changed action");
        }
        
    }
}
















