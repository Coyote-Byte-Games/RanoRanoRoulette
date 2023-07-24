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
    public Material blurMat;
    private bool modsSupressed;
    //Modifiers added during a supression
    private List<IModifier> supressedModifiers = new List<IModifier>();
    public Material blurInvertedMat;
    public EntityBaseScript entityBase;


    public Animator animator;

    public GameObject StateModBox;
    //the threshold at which slam effects occur during beach ball
    public float crashThreshold;
    //todo make hotbar box spawn multiple
    public Rigidbody2D rb;

    private Vector2 lastVelocity;

    public Vector2 OldVelocity()
    {
        return lastVelocity;
    }

    //for the tags that are to be excepted from damage collisons
    public GameObject jumpEffect;
    public GameManagerScript gameManager;
    public GameObject bootPrefab;
    public GameObject hatHolder;

    [SerializeField]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public AudioClip jumpSFX;
    public Image[] hearts;
    public GameData data;
    public List<IModifier> mods = new List<IModifier>();

    #region Primitives
    public short controlInversion = 1;
    public int maxSpeed;
    private bool magnetTime;
    public int jumpPower;
    public float speed;
    public int jumpsUsed = 0;
    public float borderDeathMax;
    private float borderDeathTimer;
    private int startingJumps = 1;
    public int extraJumps = 0;
    private float shitAcceleration;
    public float jumpRadius;
    public Dictionary<string, float> speedModifiers = new Dictionary<string, float>();
    #endregion

    #region Private / Hidden

    private float modEnableCD;

    private SpriteRenderer renderer;

    private Keyboard kb = Keyboard.current;
    private Mouse mouse = Mouse.current;
    #endregion
    BoxCollider2D bc;
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
    public void AddAccessory(GameObject accessory, string name)
    {
        var accessor = Instantiate(accessory);
        accessor.name = name;
        accessor.transform.SetParent(hatHolder.transform);
        accessor.transform.localPosition = Vector2.zero;
        accessories.Add(accessor);
        // hatHolder.transform
    }
    void Awake()
    {
        this.playerActions = new PlayerInfoV2<IPlayerAction>();
        this.playerStates = new PlayerInfoV2<IPlayerState>();
        renderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        this.jumpSFX = entityBase.soundManager.GetClip(SFXManagerSO.Sound.boing);
    }
    public void IWantRanosHead()
    {
        entityBase.die();
    }
    public void EnableSpikeBoots()
    {
        this.groundCheck.GetComponent<BoxCollider2D>().enabled = true;
        this.damagingJump = true;
        AddAccessory(bootPrefab, "boots");
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
            renderer.color = renderer.color - new Color(0, 0, 0, switcher ? 1 * ogColor.a : -1 * ogColor.a);
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
        SubscribeEventHandlers();

        //The timer that kills when getting too cozy with the border
        borderDeathTimer = borderDeathMax;

        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        // groundCheck = transform.GetChild(1).GetChild(0).gameObject.transform;

    }

    private void SubscribeEventHandlers()
    {
        entityBase.OnDeath += DeathEventHandler;
        entityBase.OnHealthChanged += HealthChangedEventHandler;
        entityBase.OnTakeDamage += TakeDamageEventHandler;
    }

    private float jumpCooldown;
    public float iFrameDuration;
    private float invincibleTimeLeft;
    private bool damagingJump = false;

    public List<GameObject> accessories = new List<GameObject>();
    //BTW I hate this so so so so so so much. Is this what Uncle Bob would've wanted?
    //These are all being passed by reference, so it's okay, right?s
    private Dictionary<string, IEnumerator> modContinuousEffects = new Dictionary<string, IEnumerator>();

    // public int Health
    // {

    //     set
    //     {

    //         for (int i = 0; i < hearts.Length; i++)
    //         {
    //             if (value <= 0)
    //             {
    //                 die();
    //             }
    //             if (i < value)
    //             {
    //                 hearts[i].enabled = true;

    //             }
    //             else
    //             {
    //                 hearts[i].enabled = false;
    //             }
    //         }
    //         _hp = value;
    //         //  lastSetHealth = value;
    //     }
    //     get { return _hp; }
    // }
    private void HealthChangedEventHandler(object sender, HealthChangedEventArgs e)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < e.newValue)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
    private void DeathEventHandler()
    {

        gameManager.CleanupForLevelReload(false);


        #region Scene Change
        gameManager.StartCoroutine(gameManager.LoadSceneStylin(((int)SceneEnum.GAMEOVER)));
        #endregion


    }

    public void AddAction(IPlayerAction action)
    {
        this.playerActions.AddItem(action);
        try
        {

            ActionHotbarAnimate(playerActions.GetItem().GetIcon());
        }
        catch (System.NullReferenceException)
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
        catch (System.Exception)
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
        catch (System.Exception)
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
        if (modsSupressed)
        {
            supressedModifiers.Add(modSO.GetModifier());
        }
        else
        {
            AddModifier(modSO.GetModifier());

        }
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
    public bool HasModiifer(IModifier mod)
    {
        if (mods.Any(item => item.GetType() == ((mod).GetType())))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool HasModiifer(Type mod)
    {
        return mods.Any(item => item.GetType() == mod);
    }
    public void AddModifier(IModifier mod)//! this may be broken, idk
    {

        if (HasModiifer(mod))
        {
            return;
        }
        if (modsSupressed)
        {
            supressedModifiers.Add(mod);
        }
        else
        {




            this.mods.Add(mod);
            mod.SetPlayer(this);
            mod.SetPermenantEffects(this);
            EnableModifier(mod);
            foreach (var preExistingMod in mods)
            {
                preExistingMod.OnNewModAdded(this);
            }
        }


        //todo end effect
        //find a way to set the player effects in the player script


    }
    public bool Grounded()
    {

        // return Physics2D.OverlapCircle(groundCheck.position - (.1f * Vector3.up), jumpRadius, groundLayer);
        return Physics2D.Raycast(transform.position, Vector2.down, jumpRadius, groundLayer);

    }
    void OnCollisionExit2D(Collision2D col)
    {

    }
    void OnCollisionEnter2D(Collision2D col)
    {

        // if (col.otherCollider.gameObject.CompareTag("FriendlyAttack"))
        // {
        //     return;
        // }
        // if (script is not null)
        // {
        //     Vector2 directionFromEnemy;
        //     if (script.knockBackOverride != Vector2.zero)
        //     {
        //         directionFromEnemy = Vector2.right;
        //     }
        //     else
        //     {
        //         directionFromEnemy = (rb.position - col.rigidbody.position).normalized;
        //     }
        //     if (damagingJump && directionFromEnemy.y > .6f)
        //     {
        //         // string[] msgs = {"Miss!", "Dodge!", "Parry!", "Nat 20!" };
        //         // var msg = msgs[ UnityEngine.Random.Range(0, msgs.Length)];
        //         // CreatePopup(msg);
        //         Jump(false, true);

        //         animator.SetTrigger("JumpTrick");
        //         jumpsUsed = 0;
        //         var entityBaseScript = script.GetComponent<EntityBaseScript>();
        //         if (entityBaseScript != null)
        //         {
        //             entityBaseScript.TakeDamage(1, false);
        //         }

        //         return;
        //     }

        //     if (!unmovable)
        //     {
        //         rb.AddForce(directionFromEnemy * 99999 / 100 * script.GetKB() * Time.deltaTime);

        //     }

        //     if (invincibleTimeLeft > 0)
        //     {
        //         return;
        //     }
        //     if (!unkillable)
        //     {
        //          entityBase.TakeDamage(script.GetDamage(), true);

        //     }
        //     // TakeDamage(script.GetDamage(), true);

        // }
        // if ((bool)col.otherCollider.gameObject.GetComponent<EnemyTraits>())
        // {
        //      Vector2 directionFromEnemy = (rb.position - col.rigidbody.position).normalized;
        // if (damagingJump && directionFromEnemy.y > .6f)
        //     {
        //         // string[] msgs = {"Miss!", "Dodge!", "Parry!", "Nat 20!" };
        //         // var msg = msgs[ UnityEngine.Random.Range(0, msgs.Length)];
        //         // CreatePopup(msg);
        //         Jump(false, true);

        //         animator.SetTrigger("JumpTrick");
        //         jumpsUsed = 0;
        //         var entityBaseScript = col.gameObject.GetComponent<EntityBaseScript>();
        //         if (entityBaseScript != null)
        //         {
        //             entityBaseScript.TakeDamage(1, false);
        //         }

        //         return;
        //     }
        // }

    }

    public void TakeDamageEventHandler()
    {


        invincibleTimeLeft = iFrameDuration;

        StartCoroutine(IFrameFlicker());

    }
    public void OnTriggerStay2D(Collider2D col)
    {
        var sticky =  col.gameObject.GetComponent<StickyPointScript>();
        if (sticky is not null)
        {
            //add logic for sticking
        }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
       
        var dialougeScript = col.gameObject.GetComponent<DialougeScript>();
        if (dialougeScript is not null)
        {
            dialougeScript.BeginDialougeFromStart();
        }
        var supression = col.gameObject.GetComponent<ModSupressionFieldScript>();
        //Handle Modifier Supression Field
        if (supression is not null)//Collision
        {
            modsSupressed = true;
            GetComponent<BuffableBehaviour>().CreatePopup("Clean, at last.", BuffableBehaviour.BuffIcon.Clean);
            StopAllCoroutines();
            foreach (var item in mods)
            {
                DisableModifier(item);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        var supression = col.gameObject.GetComponent<ModSupressionFieldScript>();
        //Handle Modifier Supression Field
        if (supression is not null)//Collision
        {
            GetComponent<BuffableBehaviour>().CreatePopup("Impurity Reclaimed.");

            Debug.Log("Left supression field");
            modsSupressed = false;
            foreach (var item in supressedModifiers)
            {
                AddModifier(item);
            }
            //Add the modifiers only once we leave the supression zone
            if (modEnableCD < 0)
            {
                modEnableCD = .2f;
                mods.ForEach(x => EnableModifier(x));

            }
        }
    }

    private void EnableModifier(IModifier mod)
    {

        mod.OnStartEffect(this);
        RegisterAndScheduleModContFX(mod);



    }

    private void RegisterAndScheduleModContFX(IModifier mod)
    {

        //Should be fine to do this as:
        //delegates are classes
        //Arg is a Func, a delegate and thus a class
        //classes are passed by reference, unlike structs passed by value 
        //right?!?!

        //AAAAAAAAAGHHHH
        if (!this.modContinuousEffects.ContainsKey(mod.ToString()))
        {
            this.modContinuousEffects.Add(mod.ToString(), mod.ContinuousEffect(this));
        }

        //hahahah FML
        //lets just make sure why dont we
        StopCoroutine(modContinuousEffects[mod.ToString()]);
        StartCoroutine(BeginModifierContinuousEffct(mod.ToString()));

        // StartCoroutine(modContinuousEffects[mod.ToString()]);

    }

    private IEnumerator BeginModifierContinuousEffct(string v)
    {
        for (; ; )
        {
            //todo fix this when it isn't 2:49AM


            yield return new WaitForSeconds(3f);
            StopCoroutine(modContinuousEffects[v]);
            StartCoroutine(modContinuousEffects[v]);
            yield break;
        }
    }

    private void DisableModifier(IModifier mod)
    {
        mod.OnEndEffect(this);
        //HUH?!?!?!?!?!?!??!?!?!?!?!?!?!?!
        if (!this.modContinuousEffects.ContainsKey(mod.ToString()))
        {
            this.modContinuousEffects.Add(mod.ToString(), mod.ContinuousEffect(this));
        }
        StopCoroutine(modContinuousEffects[mod.ToString()]);
    }

    public float GetHomemadeAcceleration()
    {
        return shitAcceleration;
    }
    public void Respawn(Vector3 destination)
    {
        StartCoroutine(gameManager.CreateRano(destination));
    }
    private void FixedUpdate()
    {
        // if (!modsSupressed)
        // {
        //     foreach (var item in supressedModifiers)
        //     {
        //         AddModifier(item.GetModifier());
        //     }
        // }
        for (int i = 0; i < hatHolder.transform.childCount; i++)
        {
            hatHolder.transform.GetChild(i).transform.localPosition = Vector2.zero;
        }

        // Vector2 velocity = rb.velocity;
        // Vector2 deltaVector = velocity - lastVelocity;
        // shitAcceleration = (deltaVector).magnitude / Time.fixedDeltaTime;
        // float dif = Mathf.Abs(shitAcceleration);

        // if (dif > maxChange && Mathf.Abs(deltaVector.y) > 5)
        // {
        //     // rb.velocity += lastVelocity * Vector2.right;
        // }

        // lastVelocity = velocity;
    }
    //tracking for the Slam that occurs when rano hits something at a high velocity
    // private Vector2 oldDirection;
    void Update()
    {

        modEnableCD -= Time.deltaTime;

        if (transform.position.y < -50)
        {
            entityBase.die();
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
            jumpsUsed = 0;
        }
        if (!Grounded())
        {
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

        rb.AddForce(new Vector2(horizontal * speed * GetSpeedModifier() * controlInversion * Time.deltaTime, 0f));
        // rb.velocity = (new Vector2(horizontal * speed * GetSpeedModifier() * controlInversion, rb.velocity.y));
       
        animator.SetFloat("speed", horizontal);

        #region Magnetic
        //To stop rano from contantly bouncing as he runs, a raycast will keep him grounded until he jumps.
        // if (Physics2D.Raycast(groundCheck.position, Vector2.down, .1f, groundLayer) && magnetTime)
        // {
        //     rb.velocity = rb.velocity * Vector2.right;
        // }
        #endregion


        var renderer = transform.GetChild(1).GetComponent<SpriteRenderer>();

        bool flippin = false;
        bool doingAnything = false;
        switch (horizontal)
        {
            case 1:
                flippin = false;
                doingAnything = !false;
                break;
            case -1:
                flippin = true;
                doingAnything = !false;
                break;
            default:
                //do not modify the turn if no input
                doingAnything = false;
                break;

        }
        foreach (var item in renderer.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            if (doingAnything)
            {
                item.flipX = flippin;

            }

        }


        //Core jump logic
        if ((GetJumpsAvailable() > 0) && kb.spaceKey.wasPressedThisFrame && !(jumpCooldown > 0))
        {
            // rb.velocity += new Vector2(0, ( jumpPower*2000 * Time.deltaTime));

            animator.SetTrigger("Jump");
            foreach (var item in GetComponentsInChildren<Animator>())
            {
                item.SetTrigger("Jump");
            }
            jumpCooldown = .02f;

            AudioClip correctJumpSFX = (Grounded()) ? jumpSFX : entityBase.soundManager.GetClip(SFXManagerSO.Sound.whoosh);
            //Jump's sfx can be overridden
            Jump(!Grounded(), jumpSFX: correctJumpSFX);




        }
    }

    private float GetSpeedModifier()
    {
        float product = 1;
        try
        {
            product = speedModifiers.Values.Aggregate((a, x) => a * x);
        }
        catch (System.Exception)
        {

        }
        return product;
    }

    public int GetJumpsAvailable()
    {
        if (Grounded())
        {
            return startingJumps + extraJumps;
        }
        else
        {
            return (extraJumps) - jumpsUsed;
        }
    }

    //fixme jank as hell
    public void Jump(bool useJumpCredit, bool overrideBerserk = false, bool playSound = true, AudioClip jumpSFX = null)
    {

        if (useJumpCredit)
        {
            jumpsUsed++;

        }
        if (overrideBerserk && HasModiifer(typeof(BerserkModifier)))
        {
            rb.velocity =
               rb.velocity.x * Vector2.right +
               Vector2.up * jumpPower * BerserkModifier.jumpDivider;
        }
        else
        {
            rb.velocity =
               rb.velocity.x * Vector2.right +
               Vector2.up * jumpPower;
        }



        //Old jump; reworked since /v2.3
        // rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        magnetTime = false;


        Instantiate(jumpEffect, groundCheck.position + .8f * Vector3.up, Quaternion.identity);
        StartCoroutine(TickMagnetCooldown());
        if (!playSound)
        {
            return;
        }
        AudioClip correctSFX = jumpSFX ? jumpSFX : entityBase.soundManager.GetClip(SFXManagerSO.Sound.boing);
        entityBase.AS.PlayOneShot(correctSFX);

    }

    private IEnumerator TickMagnetCooldown()
    {
        for (; ; )
        {

            yield return new WaitForSeconds(.1f);
            magnetTime = true;
            yield break;
        }
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
        renderer.sprite = sprite;
    }

    public void AddState(IPlayerState state)
    {
        this.playerStates.AddItem(state);
        try
        {
            StateHotbarAnimate(playerStates.GetItem().GetIcon());
        }
        catch (System.Exception)
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

    internal void RemoveAction(IPlayerAction action)
    {
        this.playerActions.RemoveItem(action);
        try
        {

            ActionHotbarAnimate(playerActions.GetItem().GetIcon());
        }
        catch (System.NullReferenceException)
        {
            return;
        }
    }

    internal void RemoveSpikeBoots()
    {
        this.groundCheck.GetComponent<BoxCollider2D>().enabled = false;
        this.damagingJump = false;
        RemoveAccessory("boots");
    }

    internal void RemoveAccessory(string v)
    {
        var fugitive = accessories.Find(x => x.name == v);
        accessories.Remove(fugitive);
        Destroy(fugitive);
    }
}

