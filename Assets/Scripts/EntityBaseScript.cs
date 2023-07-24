using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthChangedEventArgs : EventArgs
{
    public int newValue;
    public HealthChangedEventArgs(int newVal)
    {
        newValue = newVal;
    }
}
public class EntityBaseScript : MonoBehaviour
{
    private int _hp;
    public bool unmovable = false;
    public bool unkillable = false;
    private float invincibleTimeLeft;
    public float iFrameDuration = 0;

    [Space]

    //todo: fix the jank ass buff system
    public List<string> damagingTags;
    public LayerMask damagingLayers;

    public SFXManagerSO soundManager;
    #region events
    public event Action OnDeath;
    public event Action OnTakeDamage;
    public event EventHandler<HealthChangedEventArgs> OnHealthChanged;
    public SpriteRenderer render;

    #endregion

    #region Hidden/Private
    [HideInInspector]
    public AudioSource AS;
    private bool dead;
    public int maxHP;
    private Rigidbody2D rb;

    private GameObject boom;
    #endregion

    public int Health
    {

        set
        {
            OnHealthChanged?.Invoke(this, new HealthChangedEventArgs(value));
            if (value <= 0)
            {
                die();
            }


            _hp = value;
            //  lastSetHealth = value;
        }
        get { return _hp; }
    }


    internal AudioSource GetAudioSource()
    {

        AS = gameObject.AddComponent<AudioSource>();

        return AS;
    }


    public void TakeDamage(int v, bool iFramesEnabled)
    {

        if (v < 0 || invincibleTimeLeft > 0)
        {
            return;
        }
        Health -= v;
        Debug.Log(name + " took damage");
        OnTakeDamage?.Invoke();
        if (iFramesEnabled)
        {
            StartCoroutine(IFrameFlicker());
            invincibleTimeLeft = iFrameDuration;

        }

    }
    public IEnumerator IFrameFlicker()
    {
        Debug.Log("flickering");
        bool switcher = true;
        var ogColor = render.color;
        while (invincibleTimeLeft > 0)
        {
            // render.color = render.color - new Color(0, 0, 0, switcher ? 1 : -1);
            render.color = ogColor - new Color(0, 0, 0, switcher ? 1 : 0);
            switcher = !switcher;
            yield return new WaitForSeconds(.1f);
        }
        // this.transform.GetChild(1).GetComponent<SpriteRenderer>().color = this.transform.GetChild(1).GetComponent<SpriteRenderer>().color + new Color(0,0,0,1);
        render.color = ogColor + new Color(0, 0, 0, 1);
        yield break;

    }

    internal virtual void die()
    {

        if (dead)
        {
            return;
        }

        //As? as what?
        // FindObjectOfType< GameManagerScript>().audioSource.PlayOneShot(soundManager.GetClip(SFXManagerSO.Sound.dodgeball)); //kaboom

        dead = true;
        OnDeath?.Invoke();
        try
        {
            var npcness = GetComponent<DialougeScript>();
            //cant talk if youre dead moron
            npcness.EndConversation();
        }
        catch (System.Exception)
        {
            //yeah im just gonna do nothing how do you like that bitch your "clean code" can go straight to hell
        }

        //Unable to play its own SFX as it is dying

        var kablooey = Instantiate(boom, transform.position, Quaternion.identity);
        kablooey.GetComponent<AudioSource>().PlayOneShot(soundManager.GetClip(SFXManagerSO.Sound.boom));



        // Destroy(kablooey, .25f);
        Destroy(gameObject);
    }


    public virtual void Start()
    {

        
        AS = gameObject.AddComponent<AudioSource>();
       
        rb = GetComponent<Rigidbody2D>();
        this.Health = maxHP;

    }
    public void Awake()
    {
        boom = (Resources.Load("Prefabs/boom") as GameObject);
    }
    public void OnCollisionEnter2D(Collision2D other)
    {

        bool getsDamaged = damagingLayers == (damagingLayers | (1 << other.gameObject.layer)); // damagingTags.Any(x => x == other.gameObject.tag) || damagingLayers.Any(x => x == other.gameObject.layer) ;
        if (getsDamaged && !unkillable)
        {
            var script = other.gameObject.GetComponent<DamagingBehaviour>();
            //todo add dynamic damage number
            TakeDamage(script.GetDamage(), true);


            if (script is not null)
            {

                Vector2 directionFromEnemy;
                if (script.knockBackOverride != Vector2.zero)
                {
                    directionFromEnemy = Vector2.right;
                }
                else
                {
                    directionFromEnemy = (rb.position - other.rigidbody.position).normalized;
                }


                if (!unmovable)
                {
                    rb.AddForce(directionFromEnemy * 99999 / 100 * script.GetKB() * Time.deltaTime);

                }

                if (invincibleTimeLeft > 0)
                {
                    return;
                }


            }
        }









    }
    public void Update()
    {
        invincibleTimeLeft -= Time.deltaTime;
    }

    // Start is called before the first frame update
}


