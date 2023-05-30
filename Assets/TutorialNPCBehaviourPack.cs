using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TutorialNPCBehaviourPack : EnemyTraitScript, INPCBehaviourPack
{
    public Action[] CustomEvents;//its just so important i had to use the thing
    // Start is called before the first frame update
    void Start()
    {
         CustomEvents = new Action[]
    {
        () => TeleportAway()
    };
    }
    
     public void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
    }
    public void RunCustomEvent(int index)
    {
        CustomEvents[index]?.Invoke();
    }
    // Update is called once per frame
 internal override void die()
 {

    MenuScript.instance.LoadSceneSoSoSoftly(SceneEnum.MainMenu);
    FindAnyObjectByType<RanoScript>().IWantRanosHead();
    base.die();
 } 
    private void TeleportAway()
    {
        this.GetComponent<Rigidbody2D>().position += new Vector2(25,5);
    }
}
