using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Modifier Manager")]
public class ModifierManager : ScriptableObject
{

private static System.Random rng;



    //I honestly don't like how verbose this is, especially since it's gonna turn into a mess of Linq
    public List<List<Type>> mutualExlusives = new List<List<Type>>
    {
        new List<Type>{typeof(BeachBallModifier),typeof(BerserkModifier) },
        new List<Type>{typeof(BeachBallModifier),typeof(SpikedShoesModifier)},
    };
    private RanoScript player;

    public GameData gameData;
    public ModifierSO[] modSOs;

    [Space]
    public int startingModNumber;
    public int numOfMods;
    public int difficulty;
    public bool startUpDelayFinished = false;
    public int modifierInterval = 15;
    ///<summary>The mods that may be distributed randomly with the wheel. Global, and may be changed based on conditions such as difficulty. Subject to change.</summary>

    public List<IModifier> mods = new List<IModifier>();

// private IList<T> Shuffle<T>(IList<T> list)  
// {  
// System.Random localRng = new System.Random(GameConfig.GetSeed());  

//     int n = list.Count;  
//     while (n > 1) {  
//         n--;  
//         int k = localRng.Next(n + 1);  
//         T value = list[k];  
//         list[k] = list[n];  
//         list[n] = value;  
//     }  
//     return list;
// }

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
    public void OnEnable()
    {
        numOfMods = startingModNumber;
        //If the game is released and not in testing, use the settings. Otherwise, the value in the editor will take place.
        #if !UNITY_EDITOR
        modifierInterval = SettingsScript.modInterval;
        #endif
        //Because of the way Rano is spawned, haVing the mods start before Rano himself even spawns breaks the game. This also stops the game from being totally destroyed.
        if (modifierInterval < 3)
        {
            modifierInterval = 3;
        }
    }
    public void Reset()
    {
        numOfMods = startingModNumber;
    }
    public void NabCommonMods(ModifierSO[] collection)
    {
        rng = new System.Random(GameConfig.GetSeed());  

        collection = collection.OrderBy(_ => rng.Next()).ToArray();//(ModifierSO[])Shuffle(collection);

        
   
        //mods that cannot exist thanks to mutual exlusives
        List<Type> blackList = new List<Type>();


        List<IModifier> copy = new List<IModifier>();
        foreach (var item in collection)
        {
            var mod = item.GetModifier();
            if (blackList.Contains(mod.GetType()))
            {
                Debug.Log("caught moron " + mod.GetType());
                continue;
            }




            copy.Add(mod);
            IEnumerable modsInExclusives = new List<IModifier>();
            try
            {
                modsInExclusives = mutualExlusives
                .Where(thing => thing.Any(x => x == mod.GetType()))
                .Select(thing => thing).Aggregate((a, b) => a.Concat(b).ToList());
            }
            catch (System.Exception)
            {
                //do nothin'
            }

            //concat is like addrange but it returns a value
                Debug.Log(modsInExclusives.GetType());

            foreach (Type shitter in modsInExclusives)
            {
                Debug.Log("here is the sauce, boss " + shitter.ToString());
                blackList.Add(shitter);
            }


            //a list of pairs that describe mutual exclusive mods. Get all the pairs that contain this mod, and remove ALL mods in that list (since we aren't going to use the one we've found here again)


        }
        commonMods.AddRange(copy);
        commonMods.ForEach(x => Debug.Log("new spice " + x));
    }

    public static List<IModifier> commonMods = new List<IModifier>()


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


    // public void OnEnabled()
    // {
    //     this.NabCommonMods(modSOs);
    // }
     public int GetNumOfMods()
    {
        // return new List<int> { numOfMods, commonMods.Count }.Min();
        return numOfMods;
    }
    

    public List<IModifier> GenerateRandomMods(int numOfMods)
    {
        var output = new List<IModifier>();
        List<IModifier> copy = commonMods;

        //A the desired mod count is below the mods available
        //B we don't have enough mods to satisfy the demand, so we simply lower the demand
        int threshold = GetNumOfMods();

        for (int i = 0; i < threshold; i++)
        {

            int n = UnityEngine.Random.Range(0, copy.Count);//why is it count and not length why can you not be normal bill gates is dead to me


            //attempted fix at duplicate modifiers
            IModifier item;

            item = copy[(n)];



            output.Add(copy[n]);
            copy.RemoveAt(n);
        }


        // foreach (var item in output)
        // {
        // Debug.Log($"The mods are the following:{item.ToString()}\n");

        // }
        output.ToList().ForEach(x => Debug.Log("modifier x: " + x.ToString()));
        return output;

    }


}
