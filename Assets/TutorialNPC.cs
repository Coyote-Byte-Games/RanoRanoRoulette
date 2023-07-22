using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TutorialNPC : MonoBehaviour, INPCBehaviourPack
{
    public Action[] CustomEvents;//its just so important i had to use the thing
    // Start is called before the first frame update
    void Start()
    {
        var entityBase = GetComponent<EntityBaseScript>();
        entityBase.OnDeath += DieEventHandler;
    }
 internal void DieEventHandler()
 {
    MenuScript.instance.LoadSceneSoSoSoftly(SceneEnum.MainMenu);
 }
}
