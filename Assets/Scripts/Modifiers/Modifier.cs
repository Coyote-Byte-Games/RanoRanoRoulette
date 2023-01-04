using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Modifier
{


    public string name;
    public int difficulty;
    public string description;   
    ///<summary>The mods that may be distributed randomly with the wheel. Global, and may be changed based on conditions such as difficulty. Subject to change.</summary>



    private static Modifier[] commonMods = new Modifier[]
    {
        new BerserkModifier(),
        new BerserkModifier(),
        new BerserkModifier(),
        new BerserkModifier()
    };

    public static List<Modifier> GenerateRandomMods(int numOfMods)
    {
        var output = new Modifier[numOfMods];
        List<Modifier> copy = commonMods.ToList<Modifier>();
        for (int i = 0; i < numOfMods; i++)
        {
            int n = UnityEngine.Random.Range(0, copy.Count);//why is it count and not length why can you not be normal bill gates is dead to me
            output[i] = copy[n];
            copy.RemoveAt(n);
        }


        return output.ToList();

    }

    ///<summary>Takes in a list of modifiers, and determines the behaviour for the attack of the player, whether that be choosing one, or combining them.</summary>

    #region Constructors
#nullable enable
    public Modifier(string name = "modifier", string description = "unregistered", int difficulty = 1)
    {
        this.description = description;
        this.difficulty = difficulty;
    }
#nullable disable
    #endregion Constructors


    public override string ToString()
    {
        return name;
    }

    //for interacting with any player class
    public bettertestplayablescript playerScript;
    public abstract void OnStartEffect(bettertestplayablescript player);
    public abstract IEnumerator ContinuousEffect(bettertestplayablescript player);
    public abstract void OnEndEffect(bettertestplayablescript player);
}
