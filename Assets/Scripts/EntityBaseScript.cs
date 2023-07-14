using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBaseScript : FreezableMonoBehaviour
{
    public int health = 1;
    [Space]
    public GameObject[] buffIcons;
    [Space]

    private bool dead;

    public GameObject boom;
    public GameObject popup;
    [HideInInspector]
    public AudioSource AS;
    public SFXManagerSO soundManager;

    


    public void CreatePopup(string text, int index)
    {
        var popupInst = Instantiate(popup, transform.position, Quaternion.identity);
        Destroy(popupInst, 1f);
        var script = popupInst.GetComponent<CharacterPopupScript>();
        script.SetText(text);
        script.SetImageExtra(Instantiate(buffIcons[index]));
        //someday ill make a central manager for these, Or just use resoures again


    }
    public void CreatePopup(string text)
    {
        var popupInst = Instantiate(popup, transform.position, Quaternion.identity);
        Destroy(popupInst, 1f);
        var script = popupInst.GetComponent<CharacterPopupScript>();
        script.SetText(text);
        script.SetImageExtra(null);
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
        if (dead)
        {
            return;
        }
        dead = true;
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

        Debug.Log($"Stinking it up {AS == null} {soundManager == null}");
        AS.PlayOneShot(soundManager.GetClip(SFXManagerSO.Sound.boom)); //kaboom
        // soundManager.PlayClip((SFXManagerSO.Sound.boom));


        // Destroy(kablooey, .25f);
        Destroy(gameObject);
    }


    public virtual void Start()
    {

        AS = FindAnyObjectByType<AudioSource>();


    }
    public void OnCollisionEnter2D(Collision2D other)
    {

    }
    public void Update()
    {

    }
    // Start is called before the first frame update
}


