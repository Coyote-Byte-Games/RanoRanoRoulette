using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModifierManager
{


    public string name;
    public int difficulty;
    public string description;   
    ///<summary>The mods that may be distributed randomly with the wheel. Global, and may be changed based on conditions such as difficulty. Subject to change.</summary>



    private static IModifier[] commonMods = new IModifier[]
    {
        new BerserkModifier(),
        new BerserkModifier(),
        new BerserkModifier(),
        new BerserkModifier()
    };

    public static List<IModifier> GenerateRandomMods(int numOfMods)
    {
        var output = new IModifier[numOfMods];
        List<IModifier> copy = commonMods.ToList<IModifier>();
        for (int i = 0; i < numOfMods; i++)
        {
            int n = UnityEngine.Random.Range(0, copy.Count);//why is it count and not length why can you not be normal bill gates is dead to me
            output[i] = copy[n];
            copy.RemoveAt(n);
        }


        return output.ToList();

    }

    
}
