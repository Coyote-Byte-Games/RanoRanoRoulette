using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Level Slice")]
public class LevelSlice : ScriptableObject
{
    public const int minDifficulty = 1;
    public const int maxDifficulty = 5;
    
    public GameObject enemy = null;
    public GameObject[] traps = null;
    
    public Texture2D texture;
    [Range(minDifficulty,maxDifficulty)]
    public int difficulty;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
