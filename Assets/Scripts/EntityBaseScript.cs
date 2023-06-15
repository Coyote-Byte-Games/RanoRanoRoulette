using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBaseScript : FreezableMonoBehaviour
{

    [Space]
    public GameObject[] buffIcons;
    [Space]


    public int health = 1;
    public GameObject boom;
    public GameObject popup;
    public AudioSource AS;
    public SFXManagerSO soundManager;
    public AudioClip[] SFX;


    public void CreatePopup(string text, int index)
    {
        var popupInst = Instantiate(popup, transform.position, Quaternion.identity);
        Destroy(popupInst, 1f);
        var script = popupInst.GetComponent<CharacterPopupScript>();
        script.SetText(text);
        script.SetImageExtra(Instantiate(buffIcons[index]));
        //someday ill make a central manager for these, Or just use resoures again


    }
    public virtual void TakeDamage(int v)
    {
        if ((health -= v) < 1)
        {
            die();
        }
    }
    public void GetSpeedDownGraphic()
    {

    }

    internal virtual void die()
    {

        try
        {
            var npcness = GetComponent<TalkativeNPC>();
            //cant talk if youre dead moron
            npcness.Shaddup();
        }
        catch (System.Exception)
        {
            //yeah im just gonna do nothing how do you like that bitch your "clean code" can go straight to hell
        }


        var kablooey = Instantiate(boom, transform.position, Quaternion.identity);

        AS.PlayOneShot(soundManager.GetClip(SFXManagerSO.Sound.boom)); //kaboom

        Destroy(kablooey, .25f);
        Destroy(gameObject);
    }

    public void Start()
    {
        if (AS is null)
        {
            AS = FindAnyObjectByType<AudioSource>();
        }

    }
    public void OnCollisionEnter2D(Collision2D other)
    {

        var script = other.gameObject.GetComponent<DamagingObjectScript>();

        if (script is not null)
        {
            if (script.shitlist.Contains(gameObject.layer) || true)
            {
                TakeDamage(script.GetDamage());
                Debug.Log("lmaaoooooooooooooooo im dead");
            }
        }

    }
    public void Update()
    {

    }
    // Start is called before the first frame update
}


