using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "My Assets/Levels/Level Generator V2")]
public class LevelGeneratorV2 : ScriptableObject
{
    public Vector2 startingCoordinate;
    public int xOffset = 0;
    [HideInInspector]
    public GameObject parent;
    // [HideInInspector]
    // public Tilemap Tilemap;
    // [HideInInspector]
    // public Tilemap bgTilemap;
    public LevelSliceV2[] sliceSOs;
    public GameObject flag;
    [Space]
    public int numOfChunks = 25;
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
    // public void WipeTileMap()
    // {
    //     Tilemap.ClearAllTiles();
    //     bgTilemap.ClearAllTiles();
    // }

    //Ends with all the chunks done and made
    public void GenerateLevelChunks(GameObject parent)
    {
        Vector2 spawnPosition = startingCoordinate;
        List<LevelSliceV2> slices = new List<LevelSliceV2>();
        //Get all the chunk GOs generated
        for (int i = 0; i < numOfChunks; i++)
        {
            //Getting a random index
            int index = UnityEngine.Random.Range(0, sliceSOs.Count());
            //Getting the chunk SO from that index
            // GameObject chunkGO = sliceSOs[index].slice;
            //Get the gameobject
            slices.Add(sliceSOs[index]);
        }
        //Begin placing them one by one, using their collider bounds to place them together
        for (int i = 0; i < slices.Count(); i++)
        {
            GameObject GO = slices[i].slice;

            //Get the position to spawn at
            var instance = Instantiate(GO, spawnPosition + new Vector2( slices[i].rightwardOffset, 0), Quaternion.identity, parent.transform);
            var tm = instance.GetComponentInChildren<Tilemap>();
            
            
            spawnPosition = tm.CellToWorld(new Vector3Int( tm.cellBounds.xMax, 0, 0)) + new Vector3(xOffset, 0, 0);
            // spawnPosition+= new Vector2( instance.GetComponentInChildren<TilemapCollider2D>().bounds.max.x, 0);
            Debug.Log($"spawnPosition is {spawnPosition}");
        }
        //place endpoint with coords
        Instantiate(flag, spawnPosition + 20f * Vector2.right, Quaternion.identity);
    }
}