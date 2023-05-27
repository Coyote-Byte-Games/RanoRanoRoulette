using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public Sprite pressedSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        //lazy as hell
        if (col.gameObject.GetComponent<RanoScript>() is not null)
        {
        OnPress();
            
        }
    }

    public void OnPress()
    {
        EventManager.instance.OnButtonPress(transform.parent.gameObject.GetInstanceID());
        GetComponent<SpriteRenderer>().sprite = pressedSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
