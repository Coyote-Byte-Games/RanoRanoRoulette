using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RanoScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ActionModBox;
    public AudioSource AS;
    public AudioClip[] SFX;
    public Material blurMat;
    public Material blurInvertedMat;


    public float borderDeathMax;
    private float borderDeathTimer;
    public Animator animator;
    public GameObject boom;

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
    public LayerMask borderLayer;
    public Keyboard kb = Keyboard.current;
    public Mouse mouse = Mouse.current;
    public int jumpsRemaining = 1;
    public GameManagerScript gameManager;

    public Image[] hearts;
    public GameData data;
    public float jumpRadius;
    public bool HKeyToggle;
    public List<IModifier> mods = new List<IModifier>();
    BoxCollider2D bc;
    [SerializeField]
    public Transform groundCheck;
    public int speed;
    public int maxJumps;
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
        this.playerActions = new PlayerInfoV2<IPlayerAction>();
        this.playerStates = new PlayerInfoV2<IPlayerState>();
    }
    public void IWantRanosHead()
    {
        die();
    }
    void createOutLineBlur()
    {
        GameObject trail = new GameObject();

        var renderer = trail.AddComponent<SpriteRenderer>();
        renderer.sprite = transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
        renderer.material = controlInversion == -1 ? blurInvertedMat : blurMat;
        renderer.color = new Color(1, 1, 1, .5f);
        var trailInstance = Instantiate(trail, transform.position, transform.rotation);
        Destroy(trail);
        Destroy(trailInstance, .3f);

    }
    public IEnumerator IFrameFlicker()
    {
        bool switcher = true;
        var ogColor = this.transform.GetChild(1).GetComponent<SpriteRenderer>().color;
        while (invincibleTimeLeft > 0)
        {
            this.transform.GetChild(1).GetComponent<SpriteRenderer>().color = this.transform.GetChild(1).GetComponent<SpriteRenderer>().color - new Color(0, 0, 0, switcher ? 1 : -1);
            switcher = !switcher;
            yield return new WaitForSeconds(.1f);
        }
        // this.transform.GetChild(1).GetComponent<SpriteRenderer>().color = this.transform.GetChild(1).GetComponent<SpriteRenderer>().color + new Color(0,0,0,1);
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().color = ogColor;
        yield break;

    }
    public IEnumerator GenerateTrail(int cycles, int intensity)
    {
        for (int i = 0; i < cycles; i++)
        {
            createOutLineBlur();
            yield return new WaitForSeconds(.1f / intensity);
        }
        yield break;
    }
    void SetCircleCollider()
    {
        var col = gameObject.AddComponent<CircleCollider2D>();

        GetComponent<CircleCollider2D>().radius = 2;

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
        //The timer that kills when getting too cozy with the border
        borderDeathTimer = borderDeathMax;

        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        // groundCheck = transform.GetChild(1).GetChild(0).gameObject.transform;
        this.Health = maxHP;

    }

   

    private int _hp;
    private float jumpCooldown;
    public float iFrameDuration;
    private float invincibleTimeLeft;

    public int Health
    {

        set
        {

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
            //  lastSetHealth = value;
        }
        get { return _hp; }
    }

    private void die()
    {
        gameManager.CleanupAfterRanoKeelsOver();
        var kablooey = Instantiate(boom, transform.position, Quaternion.identity);
        AS.PlayOneShot(SFX[0]); //kaboom
        Destroy(kablooey, .25f);
        Destroy(gameObject);

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
        catch (System.NullReferenceException e)
        {
            Debug.Log(e.Data.ToString());
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
            Debug.Log(playerActions.GetItem() is null ? "null is the thing" : "null is NOT the thing");
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
            StateModBox.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, .5f);
            // StateModBox.transform.GetChild(0).GetComponent<Image>().color + 

        }
        else
        {
            StateModBox.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 1f);

        }

        //   Destroy(box, 1);
    }
    public void AddModifier(ModifierSO modSO)
    {
        AddModifier(modSO.GetModifier());
    }
    public static bool IsPointerOverUIObject()
    {
        bool noUIcontrolsInUse = EventSystem.current.currentSelectedGameObject == null;
        return !noUIcontrolsInUse;
    }
    public void Freeze()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void AddModifier(IModifier mod)//! this may be broken, idk
    {

        if (mods.Any(item => item.GetType() == ((mod).GetType())))
        {
            return;
        }
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
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.layer == groundLayer)
        {
            jumpsRemaining--;
            jumpCooldown = .02f;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.otherCollider.gameObject.CompareTag("FriendlyAttack"))
        {
            return;
        }


        var script = col.gameObject.GetComponent<DamagingObjectScript>();
        Debug.Log(col.gameObject.name);
        if (script is not null)
        {
            Vector2 directionToEnemy;
            if (script.knockBackOverride != Vector2.zero)
            {
                directionToEnemy = Vector2.right;
            }
            else
            {
                directionToEnemy = (rb.position - col.rigidbody.position).normalized;
            }
            rb.AddForce(directionToEnemy * 99999 / 100 * script.GetKB() * Time.deltaTime);
            if (invincibleTimeLeft > 0)
            {
                return;
            }
            TakeDamage(script.GetDamage(), true);
            // TakeDamage(script.GetDamage(), true);

        }
    }



    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("FriendlyAttack"))
        {
            return;
        }

        var script = col.gameObject.GetComponent<DamagingObjectScript>();
        if (script is not null)
        {
            if (invincibleTimeLeft > 0)
            {
                return;
            }
            TakeDamage(script.GetDamage(), true);
            // TakeDamage(script.GetDamage(), true);

        }
    }
    //tracking for the Slam that occurs when rano hits something at a high velocity
    // private Vector2 oldDirection;
    void Update()
    {


        if (transform.position.y < -50)
        {
            die();
        }

        //so rano doesnt get caught
        this.rb.position += new Vector2(0, 1E-3f);

        if (mods.Any(item => item.GetType().GetInterfaces().Contains(typeof(IAnimationOverrideModifier))))
        {
            animator.enabled = false;
        }


        // #region speedSlam


        // float newSpeed = rb.velocity.sqrMagnitude;

        // if (oldDirection.sqrMagnitude - 300 * (300) >= newSpeed)
        // {
        //     //do the crash

        //     //Get the tan(pointer towards the direction of the body), then turn it to degrees.
        //     // + 180 degrees to reverse. Since 0 deg on transform means its facing down, but 90 deg collision would be facing right, we need to remove 90 degrees to change the behaviour
        //     //and then i screwed with the math via guess and check
        //     var direction = Mathf.Atan2(oldDirection.y, oldDirection.x) * Mathf.Rad2Deg - 240;

        //     Quaternion rotation = Quaternion.Euler(0, 0, direction);
        //     Instantiate(jumpEffect, groundCheck.position, rotation);

        // }

        // oldDirection = rb.velocity;

        // #endregion


        #region Sprite

        // SetOutlineSprite(this.GetComponentInChildren<SpriteRenderer>().sprite);

        #endregion

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        // Debug.Log(GKeyToggle);
        //handle horizontal movement
        MovementMethod();
        animator.SetBool("grounded", Grounded());

        if (Grounded() && jumpCooldown <= -1)
        {
            jumpsRemaining = maxJumps;
        }
        jumpCooldown -= Time.deltaTime;
        invincibleTimeLeft -= Time.deltaTime;


        //pain
        if (kb.qKey.wasPressedThisFrame)
        {
            OnStatusToggle();
        }
        if (kb.eKey.wasPressedThisFrame)
        {
            OnChangeStatus();
        }
        if (mouse.leftButton.wasPressedThisFrame)
        {
            if (!IsPointerOverUIObject())
            {
                OnAction();
            }
        }
        if (mouse.rightButton.wasPressedThisFrame)
        {
            OnChangeAction();
        }


    }

    #region InputManager

    public void OnAction()
    {
        try
        {
            Action();
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.Log("No actions?");
            return;
        }
    }
    public void OnChangeStatus()
    {
        ChangeState();
    }
    public void OnStatusToggle()
    {

        try
        {
            ToggleState();
        }
        catch (ArgumentOutOfRangeException)
        {
            return;
        }

    }
    public void OnChangeAction()
    {
        ChangeAction();
    }
    #endregion



    private void ToggleState()
    {
        this.playerStates.GetItem().Toggle();
        StateHotbarAnimate(playerStates.GetItem().GetIcon());
    }

    private void MovementMethod()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        rb.AddForce(new Vector2(horizontal * speed * controlInversion * Time.deltaTime, 0f));
        animator.SetFloat("speed", horizontal);


        var renderer = transform.GetChild(1).GetComponent<SpriteRenderer>();

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
                break;
        }

        //for toggling bounce on the thing


        if (jumpsRemaining > 0 && kb.spaceKey.wasPressedThisFrame && !(jumpCooldown > 0))
        {
            // rb.velocity += new Vector2(0, ( jumpPower*2000 * Time.deltaTime));

            animator.SetTrigger("Jump");
            foreach (var item in GetComponentsInChildren<Animator>())
            {
                item.SetTrigger("Jump");
            }
            jumpCooldown = .02f;

            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            //
            AudioClip correctJumpSFX = (Grounded()) ? this.SFX[1] : this.SFX[2];
            AS.PlayOneShot(correctJumpSFX);

            Instantiate(jumpEffect, groundCheck.position + 1.8f * Vector3.up, Quaternion.identity);
           
                jumpsRemaining -= 1;            
           

        }
    }





    private void TakeDamage(int v, bool iFrames)
    {


        if (v < 0 && invincibleTimeLeft > 0)
        {
            // Health = lastSetHealth;
            return;
        }
        float frameDuration = iFrames ? iFrameDuration : 0;
        Health -= v;
        invincibleTimeLeft = frameDuration;
        StartCoroutine(IFrameFlicker());


    }

    internal void UpdateSprite(Sprite sprite)
    {
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = sprite;
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
            return;
        }
    }
    internal void AddActionAndState(IPlayerAction action, IPlayerState state)
    {
        this.AddState(state);
        this.AddAction(action);
    }

    internal void Reset()
    {
        this.mods.Clear();
    }
}
