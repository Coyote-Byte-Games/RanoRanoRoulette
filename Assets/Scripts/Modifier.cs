using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier : ScriptableObject
{
    /*questions to ask:
    * -When is an instance of this class given a method? 
    *   functions cannot be held at the engine level, meaning the code along must pass along these functions.
    *   It would be better to have every modifier held in a class, a library of modifiers, and then have the wheel take these mods from said lib by a method to retreive

    *
    */
    //public string name; //already there
    public Modifier(Action<IPlayerScript> onstart, Action<IPlayerScript> onend)
    {
        this.OnStartEffect = onstart;
        this.OnEndEffect = onend;
    }
    public int difficulty;
    //for interacting with any player class
    public IPlayerScript playerScript;
    public Action<IPlayerScript> OnStartEffect;
    public Action<IPlayerScript> OnEndEffect;
}
