using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "My Assets/ Modifier Manager")]
public class ModifierManager : ScriptableObject
{

private RanoScript player;
    public GameData gameData;
    public int difficulty;
    public string description;
    ///<summary>The mods that may be distributed randomly with the wheel. Global, and may be changed based on conditions such as difficulty. Subject to change.</summary>
public void RemoveTypes(Type args)
{
    List<IModifier> killList = new List<IModifier>();
    // foreach (var interf in args)
    // {
        foreach (var item in commonMods)
        {
            if (item.GetType().GetInterfaces().Contains(args))
            {
                killList.Add(item);
            }
        // }
    }
    foreach (var item in killList)
    {
        commonMods.Remove(item);
    }

}
public void Reset()
{
    
}
    public void NabCommonMods(ModifierSO[] collection) 
    {
        List<IModifier> copy = new List<IModifier>();
        foreach (var item in collection)
        {
            //if the mod doesnt need a mouse or we have a mouse, add it
            // if (!(item is IMouseRequired) || player.gameManager.config.usingMouse)
            // {
                 copy.Add(item.GetModifier());
            // }
            //if the mod is ineligible
            // data.SetModNumber(data.numOfMods -1);
           
        }
        commonMods.AddRange(copy);

   }

    private static List<IModifier> commonMods = new List<IModifier>()
    

    {
        //  new BerserkModifier(),
        // //  new BeachBallModifier(),
        //  new InvertedControlsModifier(),
        //  new DoubleJumpModifier(),
        //  new WantedManModifier(),
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
        this.NabCommonMods(gameData.modSOs);
    }
 
    public  List<IModifier> GenerateRandomMods(int numOfMods)
    {
        var output = new IModifier[numOfMods];
        List<IModifier> copy = commonMods.ToList<IModifier>();
       
        for (int i = 0; i < numOfMods; i++)
        {
            int n = UnityEngine.Random.Range(0, copy.Count);//why is it count and not length why can you not be normal bill gates is dead to me
            
            var item = copy[n];
           
                output[i] = copy[n];
          


          
            copy.RemoveAt(n);
        }


        return output.ToList();

    }


}
