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


    #region Fields and Properties
#region weird stuff
    [Header("Run")]
	public float runMaxSpeed; //Target speed we want the player to reach.
	public float runAcceleration; //Time (approx.) time we want it to take for the player to accelerate from 0 to the runMaxSpeed.
	[HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
	public float runDecceleration; //Time (approx.) we want it to take for the player to accelerate from runMaxSpeed to 0.
	[HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
	[Space(10)]
	[Range(0.01f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
	[Range(0.01f, 1)] public float deccelInAir;
	public bool doConserveMomentum;


#endregion

    #region UI Related-Items
    [Header("UI Stuff")]
    public GameObject ActionModBox;
    public GameObject StateModBox;
    public Image[] hearts;

    #endregion
    #region Children of Rano
    [Header("Children of Rano")]
    public List<GameObject> accessories = new List<GameObject>();
    public Transform groundCheck;
    private SpriteRenderer renderer;
    BoxCollider2D bc;
    public GameObject hatHolder;
    public EntityBaseScript entityBase;
    public Animator animator;
    public Rigidbody2D rb;
    [Header("Particle Systems")]
    public ParticleSystem firstJumpParticles;
    public ParticleSystem doubleJumpParticles;
    public ParticleSystem turningParticles;


    #endregion
    #region Modifiers and Actions
    [Header("Modifiers and Actions")]
    private float modEnableCD;
    private Dictionary<string, IEnumerator> modContinuousEffects = new Dictionary<string, IEnumerator>();

    private bool modsSupressed;
    public List<IModifier> mods = new List<IModifier>();
    private List<IModifier> supressedModifiers = new List<IModifier>();
    PlayerInfoV2<IPlayerAction> playerActions;
    PlayerInfoV2<IPlayerState> playerStates;

    #endregion
    #region Project Dependencies
    [Header("Scene/Project deps")]
    public GameData data;
    public Material blurMat;
    public Material blurInvertedMat;
    public GameManagerScript gameManager;
    public GameObject bootPrefab;
    public LayerMask groundLayer;
    #endregion
    #region Movement
    [Header("Movement")]
    public int maxSpeed;

    public float highJumpMultiplier = 2.5f;
    public float lowJumpMultiplier = 1.5f;
    private bool jerkin = false;
    public float jerkTime = 3;
    public float jerkMagnitude = 200;
    private float jumpCooldown;
    public float jumpRadius;
    public float iFrameDuration;
    private float invincibleTimeLeft;
    private bool damagingJump = false;
    private int startingJumps = 1;
    public int extraJumps = 0;
    public Dictionary<string, float> speedModifiers = new Dictionary<string, float>();
    public short controlInversion = 1;
    public int jumpPower;
    public float speed;
    public int jumpsUsed = 0;
    #endregion
    #region Private / Hidden
    private Keyboard kb = Keyboard.current;
    private Mouse mouse = Mouse.current;
    AudioClip _jumpSFX;
    float horizontal;
    #endregion Private / Hidden
    #endregion Fields and Properties 
    #region Methods        
    #region VFX

    public IEnumerator GenerateTrail(int cycles, int intensity)
    {
        for (int i = 0; i < cycles; i++)
        {
            createOutLineBlur();
            yield return new WaitForSeconds(.1f / intensity);
        }
        yield break;
    }
    ///<summary>
    ///Creates an blur trail for rano over a few seconds.
    ///</summary>
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
    #endregion
    #region Unity Built-in methods
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
    void Awake()
    {
        this.playerActions = new PlayerInfoV2<IPlayerAction>();
        this.playerStates = new PlayerInfoV2<IPlayerState>();
        renderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {

        for (int i = 0; i < hatHolder.transform.childCount; i++)
        {
            hatHolder.transform.GetChild(i).transform.localPosition = Vector2.zero;
        }
    }
    void Start()
    {



        #region weirder stuff
            	runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
		runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

		#region Variable Ranges
		runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
		runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
		#endregion
        #endregion



        SubscribeEventHandlers();

        //setting children
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }
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

                UseAction();
            }
        }
        if (mouse.rightButton.wasPressedThisFrame)
        {
            OnChangeAction();
        }


    }
    #endregion
    #region Event Handlers
    private void SubscribeEventHandlers()
    {
        entityBase.OnDeath += DeathEventHandler;
        entityBase.OnHealthChanged += HealthChangedEventHandler;
        entityBase.OnTakeDamage += TakeDamageEventHandler;
    }
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
    public void TakeDamageEventHandler()
    {
        invincibleTimeLeft = iFrameDuration;
        StartCoroutine(IFrameFlicker());
    }
    private void DeathEventHandler()
    {
        gameManager.CleanupForLevelReload(false);
        #region Scene Change
        gameManager.StartCoroutine(gameManager.LoadSceneStylin(((int)SceneEnum.GAMEOVER)));
        #endregion
    }
    #endregion
    #region Actions and States
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
    void UseAction()
    {
        //when the player clicks the action key, we launch the current action


        try
        {
            IPlayerAction action = playerActions.GetItem();
            if (!action.OnCoolDown())
            {
                action.Run();
            }
        }
        catch (System.Exception)
        {
        }


    }
    private void ToggleState()
    {
        this.playerStates.GetItem().Toggle();
        StateHotbarAnimate(playerStates.GetItem().GetIcon());
    }
    #endregion
    #region Modifiers, Accessories
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
    private void RegisterAndScheduleModContFX(IModifier mod)
    {
        //Should be fine to do this as:
        //delegates are classes
        //Arg is a Func, a delegate and thus a class
        //classes are passed by reference, unlike structs passed by value 
        //right?
        if (!this.modContinuousEffects.ContainsKey(mod.ToString()))
        {
            this.modContinuousEffects.Add(mod.ToString(), mod.ContinuousEffect(this));
        }
        //lets just make sure why dont we
        StopCoroutine(modContinuousEffects[mod.ToString()]);
        StartCoroutine(BeginModifierContinuousEffct(mod.ToString()));
    }
    private void EnableModifier(IModifier mod)
    {

        mod.OnStartEffect(this);
        RegisterAndScheduleModContFX(mod);

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
    public void EnableSpikeBoots()
    {
        this.groundCheck.GetComponent<BoxCollider2D>().enabled = true;
        this.damagingJump = true;
        AddAccessory(bootPrefab, "boots");
    }
    ///<summary>
    ///Adds anything considered an accessory, like a hat.
    ///</summary>
    public void AddAccessory(GameObject accessory, string name)
    {
        var accessor = Instantiate(accessory);
        accessor.name = name;
        accessor.transform.SetParent(hatHolder.transform);
        accessor.transform.localPosition = Vector2.zero;
        accessories.Add(accessor);
        // hatHolder.transform
    }
    #endregion
    #region Movement
    public bool Grounded()
    {

        // return Physics2D.OverlapCircle(groundCheck.position - (.1f * Vector3.up), jumpRadius, groundLayer);
        return Physics2D.Raycast(transform.position, Vector2.down, jumpRadius, groundLayer);

    }
    private void MovementMethod()
    {




        //If there is a change in the movement that isnt simply not moving, apply the drag

        horizontal = Input.GetAxisRaw("Horizontal");

        //cant make this up
        if (!jerkin)
        {
            
            var force = new Vector2(horizontal * speed * 500 * GetSpeedModifier() * controlInversion * Time.deltaTime, 0f);

            rb.AddForce(Vector2.ClampMagnitude(force, maxRunningSpeed));
            // rb.velocity = (new Vector2(horizontal * speed * GetSpeedModifier() * controlInversion, rb.velocity.y));

        }
        if (Grounded())
        {
            StartCoroutine(JerkTest());
        }
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

        if (kb.spaceKey.wasPressedThisFrame && !(jumpCooldown > 0))
        {
            // rb.velocity += new Vector2(0, ( jumpPower*2000 * Time.deltaTime));

            animator.SetTrigger("Jump");
            foreach (var item in GetComponentsInChildren<Animator>())
            {
                item.SetTrigger("Jump");
            }
            jumpCooldown = .02f;


            //Jump's sfx can be overridden
            Jump(!Grounded());




        }
    }

    private IEnumerator JerkTest()
    {

        for (; ; )
        {

            float tempHorizontal = horizontal;
            yield return new WaitForSeconds(0.15f);
            if (tempHorizontal != horizontal && horizontal != 0 && tempHorizontal != 0)
            {
                StartCoroutine(Jerk(horizontal));
            }
            yield break;
        }

    }

    private IEnumerator Jerk(float horizontal)
    {
        Debug.Log("Jerkin'");
        float timePassed = 0;
        for (; ; )
        {
            jerkin = true;

        turningParticles.Play();

            // rb.velocity = new Vector2(-horizontal * jerkMagnitude, 0);
            rb.velocity = (new Vector2(horizontal * jerkMagnitude, rb.velocity.y));
            rb.SetRotation(-15 * horizontal);
            yield return new WaitForSeconds(jerkTime);
            rb.SetRotation(0);

            jerkin = false;
            yield break;
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
    public void Jump(bool useJumpCredit, bool overrideBerserk = false, bool playSound = true)
    {
        if ((GetJumpsAvailable() <= 0))
        {
            return;
        }
        bool isDoubleJump = !Grounded();

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


        //Advanced gravity stuff

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (highJumpMultiplier - 1) * Time.deltaTime;

        }
        else if (rb.velocity.y > 0 && !kb.spaceKey.isPressed)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }







        //Old jump; reworked since /v2.3
        // rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

        if (isDoubleJump)
        {
            doubleJumpParticles.Play();
        }
        else
        {
            firstJumpParticles.Play();
        }
        if (!playSound)
        {
            return;
        }
        else
        {
            AudioClip correctJumpSFX = (!isDoubleJump) ? GetJumpSFX() : entityBase.soundManager.GetClip(SFXManagerSO.Sound.whoosh);
            entityBase.AS.PlayOneShot(correctJumpSFX);
        }


    }
    #endregion
    #region Utility/ I have no idea where to put this crap
    public AudioClip GetJumpSFX()
    {
        return entityBase.soundManager.GetClip(SFXManagerSO.Sound.boing);
    }
    public Collider2D GetCollider()
    {
        return bc;
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


    public float GetVel()
    {
        return rb.velocity.x;
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

    public void Respawn(Vector3 destination)
    {
        StartCoroutine(gameManager.CreateRano(destination));
    }

    internal void SetJumpSFX(AudioClip arg)
    {
        _jumpSFX = arg;
    }
    #endregion
    #endregion Methods
}

