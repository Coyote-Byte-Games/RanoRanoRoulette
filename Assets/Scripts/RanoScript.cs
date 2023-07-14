using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RanoScript : EntityBaseScript
{
    // Start is called before the first frame update
    public GameObject ActionModBox;
    public Material blurMat;
    public Material blurInvertedMat;
    private SpriteRenderer renderer;


    private float[] actionCooldowns;
    public float borderDeathMax;
    private float borderDeathTimer;
    public Animator animator;

    public GameObject StateModBox;
    //the threshold at which slam effects occur during beach ball
    public float crashThreshold;
    //todo make hotbar box spawn multiple
    public Rigidbody2D rb;
    public short controlInversion = 1;
    public int maxHP;
    public int maxSpeed;
    public bool unkillable;
    public bool unmovable;
    private Vector2 lastVelocity;
    private bool magnetTime;

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
    public float speedModifier = 1;
    public bool HKeyToggle;
    public List<IModifier> mods = new List<IModifier>();
    BoxCollider2D bc;
    [SerializeField]
    public Transform groundCheck;
    public float speed;
    public int maxJumps;

    public float maxChange;

    public LayerMask groundLayer;
    public int jumpPower;
    private float mangetTimeRemaining;
    public AudioClip jumpSFX;
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
        renderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        this.jumpSFX = soundManager.GetClip(SFXManagerSO.Sound.boing);
    }
    public void IWantRanosHead()
    {
        die();
    }
    public void EnableSpikeBoots()
    {
        this.groundCheck.GetComponent<BoxCollider2D>().enabled = true;
        this.damagingJump = true;
    }
    void createOutLineBlur()
    {
        GameObject trail = new GameObject();

        var trailRenderer = trail.AddComponent<SpriteRenderer>();
        trailRenderer.sprite = transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
        trailRenderer.material = controlInversion == -1 ? blurInvertedMat : blurMat;
        trailRenderer.color = new Color(1, 1, 1, .5f);
        var trailInstance = Instantiate(trail, transform.position, transform.rotation);
        Destroy(trail);
        Destroy(trailInstance, .3f);

    }
    public IEnumerator IFrameFlicker()
    {
        bool switcher = true;
        var ogColor = renderer.color;
        while (invincibleTimeLeft > 0)
        {
            renderer.color = renderer.color - new Color(0, 0, 0, switcher ? 1 * renderer.color.a : -1 * renderer.color.a);
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
    private bool damagingJump = false;

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


        base.die();
        #region Scene Change
        gameManager.StartCoroutine( gameManager.LoadSceneStylin(((int)SceneEnum.GAMEOVER)));
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
            return;
        }
    }

    void Action()
    {
        //when the player clicks the action key, we launch the current action
        IPlayerAction action = playerActions.GetItem();
        if (!action.OnCoolDown())
        {
            action.Run();

        }

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
            return;
        }


    }

    ///<summary>Animates the mod action popup. 
    ///</summary>
    ///<param name="icon"> Used for the display of the new mod.</param> 
    private void ActionHotbarAnimate(Sprite icon)
    {
        ActionModBox.GetComponent<Animator>().SetTrigger("Activate");
        ActionModBox.transform.GetChild(0).GetComponent<Image>().sprite = icon;

        //if the mod has a CD
        if (playerActions.GetItem().OnCoolDown())
        {
            StateModBox.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, .5f);
        }


    }

    private void StateHotbarAnimate(Sprite icon)
    {

        StateModBox.GetComponent<Animator>().SetTrigger("Activate");
        StateModBox.transform.GetChild(0).GetComponent<Image>().sprite = icon;
        //if the toggle is off
        if (!playerStates.GetItem().GetToggleState())
        {
            StateModBox.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, .5f);
        }
        else
        {
            StateModBox.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 1f);

        }
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
        foreach (var preExistingMod in mods)
        {
            preExistingMod.OnNewModAdded(this);
        }
        StartCoroutine(mod.ContinuousEffect(this));

        //todo end effect
        //find a way to set the player effects in the player script


    }
    public bool Grounded()
    {

        return Physics2D.OverlapCircle(groundCheck.position, jumpRadius, groundLayer);
        // return Physics2D.Raycast(transform.position, Vector2.down, jumpRadius, groundLayer);

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
        if (script is not null)
        {
            Vector2 directionFromEnemy;
            if (script.knockBackOverride != Vector2.zero)
            {
                directionFromEnemy = Vector2.right;
            }
            else
            {
                directionFromEnemy = (rb.position - col.rigidbody.position).normalized;
            }
            if (damagingJump && directionFromEnemy.y > .72f)
            {
                // string[] msgs = {"Miss!", "Dodge!", "Parry!", "Nat 20!" };
                // var msg = msgs[ UnityEngine.Random.Range(0, msgs.Length)];
                // CreatePopup(msg);
                Jump();
              
                animator.SetTrigger("JumpTrick");
                var entityBaseScript = script.GetComponent<EntityBaseScript>();
                if (entityBaseScript != null)
                {
                    entityBaseScript.TakeDamage(1);
                }

                return;
            }
            if (!unmovable)
            {
            rb.AddForce(directionFromEnemy * 99999 / 100 * script.GetKB() * Time.deltaTime);
                
            }
            Debug.Log(directionFromEnemy * 99999 / 100 * script.GetKB() * Time.deltaTime + " dsjfl;sadf");
            {

            }
            if (invincibleTimeLeft > 0)
            {
                return;
            }
            if (!unkillable)
            {
            TakeDamage(script.GetDamage(), true);
                
            }
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

    public void Respawn(Vector3 destination)
    {
        StartCoroutine(gameManager.CreateRano(destination));
    }
 private void FixedUpdate()
    {

        Vector2 velocity = rb.velocity;
        Vector2 deltaVector = velocity - lastVelocity;
        float dif = Mathf.Abs((deltaVector).magnitude) / Time.fixedDeltaTime;

Debug.Log("The delta " + deltaVector.y);
        if (dif > maxChange && Mathf.Abs(deltaVector.y) > 5)
        {
            // rb.velocity += lastVelocity * Vector2.right;
            Debug.Log("fall danage time ");
        }

        lastVelocity = velocity;
    }
    //tracking for the Slam that occurs when rano hits something at a high velocity
    // private Vector2 oldDirection;
    void Update()
    {

        



        if (transform.position.y < -50)
        {
            die();
        }


        #region modifiers
        if (mods.Any(item => item.GetType().GetInterfaces().Contains(typeof(IAnimationOverrideModifier))))
        {
            animator.enabled = false;
        }

        TickCooldowns();
        if (!playerActions.IsEmpty())
        {
            if (!playerActions.GetItem().OnCoolDown())
            {
                ActionModBox.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 1f);

            }
            else
            {
                ActionModBox.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, .5f);

            }
        }


        #endregion


        #region Sprite


        #endregion

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
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

        //Old horizontal movement method; reworked since /v2.3
        rb.AddForce(new Vector2(horizontal * speed * speedModifier * controlInversion * Time.deltaTime, 0f));
        // rb.velocity =
        // (Vector2.right * horizontal * speed * speedModifier * controlInversion) 
        // + (Vector2.up * rb.velocity.y); 
        animator.SetFloat("speed", horizontal);

        #region Magnetic
            //To stop rano from contantly bouncing as he runs, a raycast will keep him grounded until he jumps.
            if (Physics2D.Raycast(groundCheck.position, Vector2.down, .1f, groundLayer) && magnetTime)
            {
                rb.velocity = rb.velocity * Vector2.right;
                Debug.Log("magnet man");
            }
        #endregion


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

            Jump();
            AudioClip correctJumpSFX = (Grounded()) ? jumpSFX : soundManager.GetClip(SFXManagerSO.Sound.whoosh);
            AS.PlayOneShot(correctJumpSFX);


            jumpsRemaining -= 1;


        }
    }


    public void Jump()
    {

        

        //Old jump; reworked since /v2.3
        // rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        magnetTime = false;
        rb.velocity =
        rb.velocity.x * Vector2.right +
        Vector2.up * jumpPower;
        Debug.Log("jumpin");

        Instantiate(jumpEffect, groundCheck.position + .8f * Vector3.up, Quaternion.identity);
       StartCoroutine(TickMagnetCooldown());
    }

    private IEnumerator TickMagnetCooldown()
    {
        for (;;)
        {
            
        yield return new WaitForSeconds(.1f);
        magnetTime = true;
        yield break; 
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
    public void TickCooldowns()
    {
        foreach (var item in playerActions)
        {
            item.DecrementCD();
        }
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

    internal void SetAlpha(float v)
    {
        Debug.Log("setting alpha");

        foreach (var item in GetComponentsInChildren<SpriteRenderer>(true))
        {
            Color color = item.color;
            color.a = v;
            item.color = color;
        }
        Color tmp = renderer.color;
        tmp.a = v;
        renderer.color = tmp;
    }


}
