using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModifierManager
{

private RanoScript player;

    public string name;
    public int difficulty;
    public string description;
    ///<summary>The mods that may be distributed randomly with the wheel. Global, and may be changed based on conditions such as difficulty. Subject to change.</summary>

public void Reset()
{
    
}
    public void NabCommonMods(ModifierSO[] collection) 
    {
        List<IModifier> copy = new List<IModifier>();
        foreach (var item in collection)
        {
            copy.Add(item.GetModifier());
        }
        commonMods.AddRange(copy);

   }

    private static List<IModifier> commonMods = new List<IModifier>()
    

    {
         new BerserkModifier(),
        //  new BeachBallModifier(),
         new InvertedControlsModifier(),
         new DoubleJumpModifier(),
         new WantedManModifier(),
    //      new WantedManModifier(),

        //  new WantedManModifier(),

        //  new WantedManModifier(),

        //  new WantedManModifier(),
        //  new WantedManModifier(),
        //  new WantedManModifier(),
        //  new WantedManModifier(),
        //  new WantedManModifier(),
       

    


    };
   
    
    public void OnEnabled()
    {

    }
 
    public  List<IModifier> GenerateRandomMods(int numOfMods)
    {
        var output = new IModifier[numOfMods];
        List<IModifier> copy = commonMods.ToList<IModifier>();
        
        for (int i = 0; i < numOfMods; i++)
        {
            int n = UnityEngine.Random.Range(0, copy.Count);//why is it count and not length why can you not be normal bill gates is dead to me
            
            var item = copy[n];
            //if the mod doesnt need a mouse to work OR the player has a mouse, add it to the output
            if (!(item is ITrackpadHater) || player.gameManager.config.usingMouse)
            {
                output[i] = copy[n];
            }


          
            copy.RemoveAt(n);
        }


        return output.ToList();

    }


}
