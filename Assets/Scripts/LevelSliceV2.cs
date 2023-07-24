using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Level Slice V2")]
public class LevelSliceV2 : ScriptableObject
{
    public const int minDifficulty = 1;
    public const int maxDifficulty = 5;
    public int rightwardOffset = 0;
    
    public GameObject slice;
    [Range(minDifficulty,maxDifficulty)]
    public int difficulty;

}
