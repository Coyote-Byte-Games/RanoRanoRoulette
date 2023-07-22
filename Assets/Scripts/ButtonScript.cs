using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : EntityBaseScript
{

    private bool beenPressed = false;
    public Sprite pressedSprite;
    // Start is called before the first frame update
   
    public void OnCollisionEnter2D(Collision2D col)
    {
        //lazy as hell
        if (col.gameObject.GetComponent<RanoScript>() is not null && beenPressed == false)
        {
        OnPress();
        //seriously lazy as fuck; stops the button from going dadadadadadadadadadadadadadadadaad 
        beenPressed = true; 

        }
    }

    public void OnPress()
    {
        EventManager.instance.OnButtonPress(transform.parent.gameObject.GetInstanceID());
        GetComponent<SpriteRenderer>().sprite = pressedSprite;
        GetAudioSource().PlayOneShot(soundManager.GetClip(SFXManagerSO.Sound.click));
        GetComponent<BoxCollider2D>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
