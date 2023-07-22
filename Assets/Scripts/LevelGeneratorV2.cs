using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "My Assets/Levels/Level Generator V2")]
public class LevelGeneratorV2 : ScriptableObject
{

    // public RuleTile currentLevelTile;
    // public Tile currentLevelBGTile;
    // public GameObject[] garnishGameObjects;
    // [Range(0, 1)]
    // public float[] garnishWeights;
    // public float garnishVerticalOffset = 0;
    // public GameObject[] fossilGameObjects;
    // [HideInInspector]
    public Tilemap Tilemap;
    [HideInInspector]
    public Tilemap bgTilemap;
    public LevelSlice[] sliceTextures;

    public GameObject[] traps;
    public GameObject flag;
    [Space]
    public float[] sliceTexProbabliities;

    public int numOfChunks = 25;
    // public int minFossilDistance;
    // [Range(0, 100)]
    // public int fossilChance;
    // [Range(0, 100)]
    // public int shrubChance = 99;
    [Header("Fun Stuff")]
    public bool difficultyBasedGeneration = false;
    public bool weightBasedGeneration = false;




    // [SerializeField]
    // private GameObject trap1;
    [HideInInspector]
    public GameManagerScript gameManager;
    [Header("InterDeps")]
    public LevelManagerScript levelManager;

    public void Awake()
    {
        this.numOfChunks = SettingsScript.chunkNum;
    }
    public void WipeTileMap()
    {
        Tilemap.ClearAllTiles();
        bgTilemap.ClearAllTiles();
    }


//Gets one of the level chunks and spawns it in
public void GenerateLevelChunk()
{

}
public void GenerateEndpoint()
{

}

}