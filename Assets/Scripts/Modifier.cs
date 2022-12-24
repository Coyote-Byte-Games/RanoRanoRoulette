using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier{
    /*questions to ask:
    * -When is an instance of this class given a method? 
    *   functions cannot be held at the engine level, meaning the code along must pass along these functions.
    *   It would be better to have every modifier held in a class, a library of modifiers, and then have the wheel take these mods from said lib by a method to retreive

    *
    */
    //public string name; //already there
    //todo figure out where we do the null check
    #nullable enable
    
    public Modifier(Action<bettertestplayablescript>? onstart, Func<bettertestplayablescript, IEnumerator>? continuous, Action<bettertestplayablescript>? onend, string description = "unregistered", int difficulty = 1)
    {
        this.OnStartEffect = onstart;
        this.ContinuousEffect = continuous;//i hate myself
        this.OnEndEffect = onend;
        this.description = description;
        this.difficulty = difficulty;
    }
    public Modifier(Action<bettertestplayablescript>? onstart, Func<bettertestplayablescript, IEnumerator>? continuous, Action<bettertestplayablescript>? onend)
    {
        this.OnStartEffect = onstart;
        this.ContinuousEffect = continuous;//i hate myself
        this.OnEndEffect = onend;
    }
    #nullable disable
   
    public int difficulty;
    public string description;
    //for interacting with any player class
    public bettertestplayablescript playerScript;
    public Action<bettertestplayablescript> OnStartEffect;
    public Func<bettertestplayablescript, IEnumerator> ContinuousEffect;
    public Action<bettertestplayablescript> OnEndEffect;
}
