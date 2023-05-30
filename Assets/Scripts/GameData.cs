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
    public PhysicsMaterial2D normalMat;
    public Sprite FatRano;
    public Shader invertedShader;


    [SerializeField]
    public GameObject BerserkSword, BerserkSwordSheathed, AdoptionDog, DJWings, WantedManPrefab;

    #endregion
    //the mods the data starts with
    [SerializeField]
    private int startingModNumber;
    //the mods currently in the pool.

    [SerializeField]
    public ModifierSO[] modSOs;

    public int numOfMods;
 


    public List<IModifier> mods = new List<IModifier>();
     public void Reset()
    {
        numOfMods = startingModNumber;
    }

public void GetModNumber()
{
    
       
}
public void SetModNumber(int arg)
{
     startingModNumber = numOfMods = arg;
       
}
    public void OnEnable()
    {
        // GameConfig config = FindAnyObjectByType<GameConfig>();

        // if (!(item is IMouseRequired) || config.usingMouse)
        //     {
        //          copy.Add(item.GetModifier());
        //     }
        numOfMods = startingModNumber;
       

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