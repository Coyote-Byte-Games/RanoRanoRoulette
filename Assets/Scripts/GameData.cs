using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Game Data")]
public class GameData : ScriptableObject
{
    //the mods the data starts with
    [SerializeField]
    private int StartingModNumber;
    //the mods currently in the pool.
    
    public int numOfMods;
    
    
    public List<Modifier> mods = new List<Modifier>();
    public void OnEnable()
    {
        numOfMods = StartingModNumber;
    }
}
