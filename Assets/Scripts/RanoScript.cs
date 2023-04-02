using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RanoScript : MonoBehaviour{
    // Start is called before the first frame update
    public GameObject ActionModBox;
    public GameObject StateModBox;
    //the threshold at which slam effects occur during beach ball
    public float crashThreshold;
    //todo make hotbar box spawn multiple
    public Rigidbody2D rb;
    public short controlInversion = 1;
    public int maxHP;
    public int maxSpeed;

    //for the tags that are to be excepted from damage collisons
    public string[] tagList;
    public GameObject jumpEffect;
    public LayerMask enemyLayer;
    public GameManagerScript gameManager;
    
    public Image[] hearts;
    public GameData data;
    public float jumpRadius;
    public bool HKeyToggle;
    List<IModifier> mods = new List<IModifier>();
    BoxCollider2D bc;
    [SerializeField]
    public Transform groundCheck;
    public int speed;
    public LayerMask groundLayer;
    public int jumpPower;
    bool grounded;
    private PlayerInfoV2<IPlayerAction> playerActions = new PlayerInfoV2<IPlayerAction>();
    private PlayerInfoV2<IPlayerState> playerStates = new PlayerInfoV2<IPlayerState>();


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
    ////                 //just gotta see what excircleactly we need to deal with here
    ////             break;
    ////             case IMovementModifier:
    ////             break;
    ////             default:
    ////             throw new NotImplementedException("whoooooooooooOOOOOOPS we did [not] add in functionality for that !!!! ! ! ! ! ! Please contact HR at femboygaming2002@gmail.com");
    ////         }
    ////     }
        
     void Awake()
    {
       
    }
    void SetCircleCollider()
    {
        var col = gameObject.AddComponent<CircleCollider2D>();
       
        GetComponent<CircleCollider2D>().radius = 2;
        GetComponent<CircleCollider2D>().offset.Set(0,-1.5f);

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
        this.Health = maxHP;
       
    }
    private int _hp;
    private float jumpCooldown;

    public int Health
    {
        
        set {
             
            for (int i = 0; i < hearts.Length; i++)
            {
                if (value <= 0)
                {
                    die();
                }
                if (i < value)
                {
                    hearts[i].enabled = true;
                    
                }
                else
                {
                     hearts[i].enabled = false;
                }
            }

             _hp = value; 
            }
            get {return _hp;}
    }

    private void die()
    {
        rb.AddForce(Vector2.up * 99999999999);

        #region Scene Change
            gameManager.GameOver();
        #endregion


    }

    public void AddAction(IPlayerAction action)
    {
        this.playerActions.AddItem(action);
        try
        {
              ActionHotbarAnimate(playerActions.GetItem().GetIcon());
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
           return;
        }
    }
   
    void Action()
    {
        //when the player clicks the action key, we launch the current action
        IPlayerAction action = playerActions.GetItem();
        action.Run();
    }
    void ChangeAction()
    {
        
        playerActions.ChangeItem();
        try
        {
              ActionHotbarAnimate(playerActions.GetItem().GetIcon());
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
           return;
        }
      
        
    }
     void ChangeState()
    {
        
        playerStates.ChangeItem();
        try
        {
            var icon = playerStates.GetItem().GetIcon();
           
              StateHotbarAnimate
                (
                icon
                );
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
           return;
        }
      
        
    }

///<summary>Animates the mod action popup. 
///</summary>
///<param name="icon"> Used for the display of the new mod.</param> 
    private void ActionHotbarAnimate(Sprite icon)
    {
    //   var box =Instantiate(hotBarBox, rb.position + Vector2.right*0 + Vector2.up*3, Quaternion.Euler(0,0,0-transform.rotation.z));
    //   box.transform.SetPositionAndRotation(transform.position +  Vector3.up*70, Quaternion.identity);
    ActionModBox.GetComponent<Animator>().SetTrigger("Activate");
    ActionModBox.transform.GetChild(0).GetComponent<Image>().sprite = icon;
    // ModBox.transform.GetChild(0).GetComponent<Image>().preferredWidth =


    //   box.//:set the position upwards
   
    //   Destroy(box, 1);
    }

 private void StateHotbarAnimate(Sprite icon)
    {
    //   var box =Instantiate(hotBarBox, rb.position + Vector2.right*0 + Vector2.up*3, Quaternion.Euler(0,0,0-transform.rotation.z));
    //   box.transform.SetPositionAndRotation(transform.position +  Vector3.up*70, Quaternion.identity);
    StateModBox.GetComponent<Animator>().SetTrigger("Activate");
    StateModBox.transform.GetChild(0).GetComponent<Image>().sprite = icon;
    
    
 //if the toggle is off
            if (!playerStates.GetItem().GetToggleState())
            {
               StateModBox.transform.GetChild(0).GetComponent<Image>().color = new Color(255,255,255,.5f);
                // StateModBox.transform.GetChild(0).GetComponent<Image>().color + 
                
            }
            else
            {
               StateModBox.transform.GetChild(0).GetComponent<Image>().color = new Color(255,255,255,1f);
                
            }

    //   Destroy(box, 1);
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
     void OnCollisionEnter2D(Collision2D col)
    {
        if (col.otherCollider.gameObject.CompareTag("FriendlyAttack"))
        {
            return;
        }
        var script = col.gameObject.GetComponent<EnemyTraitScript>();
       if(script is not null)
       {
        Debug.Log("WOMAN?!?! " +  script.GetDamage());
        TakeDamage(script.GetDamage());
        Vector2 directionToEnemy = (rb.position - col.rigidbody.position).normalized;
        rb.AddForce(directionToEnemy*99999/100*script.GetKB()*Time.deltaTime);
       }
    }
    void Update()
    {


                if(rb.velocity.magnitude > maxSpeed)
         {
                rb.velocity = rb.velocity.normalized * maxSpeed;
         }
// Debug.Log(GKeyToggle);
        //handle horizontal movement
        MovementMethod();
        jumpCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F))
        {
            
            try
            {
             Action();
            }
            catch(ArgumentOutOfRangeException)
            {
            Debug.Log("No actions?");
            return;
            }
          
        }
         if (Input.GetKeyDown(KeyCode.C))
        {
            
            try
            {
             ToggleState();
            }
            catch(ArgumentOutOfRangeException)
            {
            Debug.Log("No states? idk this is gonna be real buggy bro");
            return;
            }
          
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeAction();
            Debug.Log("changed action");
        }
         if(Input.GetKeyUp(KeyCode.V))
        {
        //  HKeyToggle = !HKeyToggle;
        Debug.Log("Changed State");
        ChangeState();
         //Maybe right here, get the current modifier and use the toggle boolean to alter the effects.
          
        }
        
        
    }

    private void ToggleState()
    {
       this.playerStates.GetItem().Toggle();
        StateHotbarAnimate(playerStates.GetItem().GetIcon());
    }

    private void MovementMethod()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //creating dust effect
            Instantiate(jumpEffect, groundCheck.position, Quaternion.identity);
        }
        rb.AddForce(new Vector2(horizontal * speed * controlInversion * Time.deltaTime, 0f));


            var renderer = GetComponent<SpriteRenderer>();

        switch (horizontal)
        {
            case 1:
            renderer.flipX = false;
            break;
            case -1:
            renderer.flipX = true;
            break;
            default: 
            //do not modify the turn if no input
            return;
        }
    
        //for toggling bounce on the thing
       
        
        if(Grounded() && Input.GetKeyUp(KeyCode.Space) && !(jumpCooldown > 0))
        {
            // rb.velocity += new Vector2(0, ( jumpPower*2000 * Time.deltaTime));
            rb.AddForce( Vector2.up* jumpPower, ForceMode2D.Impulse);
            Debug.Log("the jumper");
            jumpCooldown += 2;
        }
    }

   
    private void TakeDamage(int v)
    {
       Health -= v;
    }

    internal void UpdateSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        var outlineSprites = this.transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>();
        foreach (var item in outlineSprites)
        {
            item.sprite = sprite;
        }
    }

    public void AddState(IPlayerState state)
    {
        this.playerStates.AddItem(state);
        try
        {
              StateHotbarAnimate(playerStates.GetItem().GetIcon());
        }
        catch (System.Exception e)
        {
            Debug.Log( "the error when adding the state was " + e );
           return;
        }
    }
    internal void AddActionAndState(IPlayerAction action, IPlayerState state)
    {
        this.AddState(state);
        this.AddAction(action);
    }
}
















