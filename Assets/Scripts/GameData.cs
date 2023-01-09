using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Game Data")]
public class GameData : ScriptableObject
{
    #region Prefabs
    public BerserkModifier BerserkModifier;

    [SerializeField]
        public  GameObject BerserkSword;

    #endregion
    //the mods the data starts with
    [SerializeField]
    private int StartingModNumber;
    //the mods currently in the pool.
    
    public int numOfMods;
    
    
    public List<IModifier> mods = new List<IModifier>();
    public void OnEnable()
    {
        numOfMods = StartingModNumber;
       
    }
}
