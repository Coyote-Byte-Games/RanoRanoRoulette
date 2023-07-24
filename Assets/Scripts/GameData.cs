using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Game Data")]
public class GameData : ScriptableObject
{
    #region Prefabs
    public TheWorldModifier BerserkModifier;
    public Sprite FatRanoIcon;
    public Animator ranoAnim;
    public PhysicsMaterial2D bouncyMat;
    public PhysicsMaterial2D normalMat;
    public Sprite FatRano;
    public Shader invertedShader;
    public Shader normalShader;
    public Material spriteUnlit;

    [SerializeField]
    public GameObject BerserkSword, BerserkSwordSheathed, AdoptionDog, DJWings, WantedManPrefab;

    #endregion
    //the mods the data starts with
    [SerializeField]

    //the mods currently in the pool.


 


   

public void GetModNumber()
{
    
       
}
}