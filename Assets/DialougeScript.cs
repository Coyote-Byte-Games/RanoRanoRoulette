using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialougeScript : MonoBehaviour
{
    
    public DialougeUIHost host;
    public DialougeContainer[] dialouges;

    private int dialougeIndex = 0;

    public void Start()
    {
        host = FindAnyObjectByType<DialougeUIHost>();
    }
    public void EndConversation()
    {
        host.EndConvo();
    }
    
    //For use when entering a trigger with the player
    public void BeginDialougeFromStart()
    {
        host.BeginDialouge(dialouges);
    }
    
    //  public void BeginNextDialouge()
    // {
    //     DialougeContainer chosen = dialouges[dialougeIndex++];
    //     StartDialougeContainer(chosen);
    // }
        public void BeginDialougeAtIndex(int index)
    {
        host.BeginDialouge(dialouges, index);
    }
    
    //Starts a dialougeContainer.
   
   
    
  
}
