using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Game Data")]
public class GameData : ScriptableObject
{
    #region Prefabs
    public BerserkModifier BerserkModifier;
    public Sprite FatRanoIcon;
    public Animator ranoAnim;
    public PhysicsMaterial2D bouncyMat;
    public Sprite FatRano;
    public Shader invertedShader;


    [SerializeField]
    public GameObject BerserkSword, AdoptionDog, DJWings;

    #endregion
    //the mods the data starts with
    [SerializeField]
    private int StartingModNumber;
    //the mods currently in the pool.

    public int numOfMods;
    public bool[] modToggles;
    public List<InspectorModToggle> inspectorModToggles = new List<InspectorModToggle>();

    public List<bool> getModToggles()
    {
        Debug.Log(inspectorModToggles);
        foreach (var item in inspectorModToggles)
        {
            Debug.Log(item.name);
        }
        var query = from item in inspectorModToggles select item.enabled;  

        Debug.Log(query);
        foreach (var item in query)
        {
            Debug.Log(query);
        }


        return (List<bool>)query;
    }

    public List<IModifier> mods = new List<IModifier>();
    public void AssignInspectorModToggles(string[] names, bool[] enables)
    {
        int i = 0;
        foreach (var item in inspectorModToggles)
        {
            item.SetName(names[i]);
            item.SetEnable(enables[i]);

            i++;
        }
       
    }

    public void OnEnable()
    {
        numOfMods = StartingModNumber;
        ModifierManager.AssignModToggles(this.inspectorModToggles);

        //assign inspector mod toggles

        //     mods = new List<IModifier>
        // {
        //     new BerserkModifier(),
        //     new BeachBallModifier()

        // };

    }
}
[System.Serializable]
public struct InspectorModToggle
{
    public string name;
    public bool enabled;
    // public IModifier mod;//not relevant atm
    public void SetName(string name)
    {
        this.name = name;
    }

    internal void SetEnable(bool v)
    {
       this.enabled = v;
    }

    public InspectorModToggle(string name)
    {
        this.enabled = false;
        this.name = name;
    }
}