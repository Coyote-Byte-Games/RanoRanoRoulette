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
        //  new BerserkModifier(),
        //  new BeachBallModifier(),
        //  new InvertedControlsModifier(),
         new AdoptionModifier(),
         new AdoptionModifier(),
         new AdoptionModifier(),
         new AdoptionModifier(),
         new AdoptionModifier(),


    };
    public static IModifier[] GetModsToggledOn(IModifier[] mods, List<bool> toggles)
    {

        List<IModifier> output = new List<IModifier>();
        //remove the mods in the commonMods
        for (int i = 0; i < toggles.Count; i++)
        {
            if (toggles[i] == true)
            {
                output.Add(mods[i]); 
            }
        }
        // Debug.Log(output);
         return output.ToArray<IModifier>();
    }
    public void OnEnabled()
    {
        
    }
    public static void AssignModToggles(List<InspectorModToggle> modTogs)
    {
        foreach (var item in commonMods)
        {
            modTogs.Append
            (
                new InspectorModToggle(item.ToString())
            );
        }
    }

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
