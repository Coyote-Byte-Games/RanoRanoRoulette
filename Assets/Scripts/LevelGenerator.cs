using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "My Assets/Levels/Level Generator")]
public class LevelGenerator : ScriptableObject
{

    public RuleTile currentLevelTile;
    public Tile currentLevelBGTile;
    public GameObject[] garnishGameObjects;
    public float garnishVerticalOffset = 0;
    public GameObject[] fossilGameObjects;
    [HideInInspector]
    public Tilemap Tilemap;
    [HideInInspector]
    public Tilemap bgTilemap;
    public LevelSlice[] sliceTextures;

    public GameObject[] traps;
    public GameObject flag;
    [Space]
    public float[] sliceTexProbabliities;

    public int numOfChunks = 25;
    public int minFossilDistance;
    [Range(0, 100)]
    public int fossilChance;
    [Range(0, 100)]
    public int shrubChance = 99;
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

    public void GenerateLevelChunks(int numOfChunks)
    {
        var slices = GenerateRandomChunkSlicesAsLevelSlices(numOfChunks);
        for (int i = 0; i < numOfChunks; i++)
        {
            GenerateLevelChunk(slices, i); //I really don't know man
        }
        //DRY
        // FindAnyObjectByType<MonoBehaviour>().StartCoroutine(
        // Scan());
    }
    public void GenerateLevelChunksWithEndpoint(int numOfChunks)
    {
        var slices = GenerateRandomChunkSlicesAsLevelSlices(numOfChunks);
        int i = 0;
        while (i < numOfChunks)
        {
            GenerateLevelChunk(slices, i); //I really don't know man

            i++;
        }
        GenerateLevelEndpoint(slices, i - 1);
        //       FindAnyObjectByType<MonoBehaviour>().StartCoroutine(
        // Scan());


    }
    public void GenerateLevelChunksWithEndpoint()
    {
        var slices = GenerateRandomChunkSlicesAsLevelSlices(numOfChunks);
        int i = 0;
        while (i < numOfChunks)
        {
            GenerateLevelChunk(slices, i); //I really don't know man

            i++;
        }
        GenerateLevelEndpoint(slices, i - 1);
        //       FindAnyObjectByType<MonoBehaviour>().StartCoroutine(
        // Scan());


    }
    private IEnumerator Scan()
    {
        yield return new WaitForSeconds(2f);
        AstarPath.active.Scan();
        yield break;
    }
    /// <summary>  
    /// <c>This version for only textures, good if you don't need more information like what enemies or traps to spawn, etc</c>
    /// </summary>

    public Texture2D[] GenerateRaStinkyndomChunkSlicesAsTextures(int count)
    {



        Texture2D[] output = new Texture2D[count];
        for (int i = 0; i < count; i++)
        {



            #region Advanced Difficulty scaling 
            //   List<int> blackList = new List<int>();
            //         List<int> difficultiesRegistered = new List<int>();
            //         //god damn it im not smart enough for cool linq shit

            //         List<int> temp = new List<int>();
            //         foreach (var item in sliceTextures)
            //         {
            //             temp.Add(item.difficulty);
            //         }
            //         difficultiesRegistered = (temp.Distinct().ToList());

            //         // int index = UnityEngine.Random.Range(0, sliceTextures.Length);//!it says this is inclusive but also exlclusive ig
            //         // Texture2D slice = sliceTextures[index].texture;
            //         // output[i] = slice;
            //         int difficulty;

            //         //old way that takes all difficulties
            //         // difficulty = UnityEngine.Random.Range(LevelSlice.minDifficulty,LevelSlice.maxDifficulty+1);

            //         //pretty much gets a random difficulty from the ones registered to use
            //         difficulty = difficultiesRegistered.ToArray()[UnityEngine.Random.Range(0, difficultiesRegistered.Count())];


            //         var query = sliceTextures.Select((tex, index) => new { tex, index })
            //                              .Where(item => item.tex.difficulty == difficulty)
            //                              .Select((tex) =>
            //                                         new
            //                                         {
            //                                             Texture = tex.tex.texture,
            //                                             Difficulty = tex.tex.difficulty
            //                                         }
            //                                         );



            //         int selection = UnityEngine.Random.Range(0, query.Count() - 1);

            #endregion



            output[i] = sliceTextures[UnityEngine.Random.Range(0, sliceTextures.Count())].texture;
            //maybe this works?

        }
        return output;
    }
















    public LevelSlice[] GenerateRandomChunkSlicesAsLevelSlices(int count)
    {
        LevelSlice[] output = new LevelSlice[count];
        for (int i = 0; i < count; i++)
        {
            #region Advanced Difficulty scaling 
            //   List<int> blackList = new List<int>();
            //         List<int> difficultiesRegistered = new List<int>();
            //         //god damn it im not smart enough for cool linq shit

            //         List<int> temp = new List<int>();
            //         foreach (var item in sliceTextures)
            //         {
            //             temp.Add(item.difficulty);
            //         }
            //         difficultiesRegistered = (temp.Distinct().ToList());

            //         // int index = UnityEngine.Random.Range(0, sliceTextures.Length);//!it says this is inclusive but also exlclusive ig
            //         // Texture2D slice = sliceTextures[index].texture;
            //         // output[i] = slice;
            //         int difficulty;

            //         //old way that takes all difficulties
            //         // difficulty = UnityEngine.Random.Range(LevelSlice.minDifficulty,LevelSlice.maxDifficulty+1);

            //         //pretty much gets a random difficulty from the ones registered to use
            //         difficulty = difficultiesRegistered.ToArray()[UnityEngine.Random.Range(0, difficultiesRegistered.Count())];


            //         var query = sliceTextures.Select((tex, index) => new { tex, index })
            //                              .Where(item => item.tex.difficulty == difficulty)
            //                              .Select((tex) =>
            //                                         new
            //                                         {
            //                                             Texture = tex.tex.texture,
            //                                             Difficulty = tex.tex.difficulty
            //                                         }
            //                                         );



            //         int selection = UnityEngine.Random.Range(0, query.Count() - 1);

            #endregion
            if (i == 0)
            {
                output[i] = sliceTextures[0];
            }
            else
            {
                output[i] = sliceTextures[UnityEngine.Random.Range(0, sliceTextures.Count())];
            }

        }
        return output;
    }













    public void GenerateLevelEndpoint(Texture2D[] slices, int iteration = 0)
    {

        //iteration is iteration; controls an x offset, basically
        var slice = slices[iteration];
        // get a slice to generate from

        //Generate blocks based off of
        //The slice image
        //The current level theme

        //2D for loop, going through a row, then proceeding down 

        int relativeZero = (from texture in slices.Take(iteration).ToList<Texture2D>()
                            select texture.width)
                            .Sum(); //I did this at 1 in the morning and made a mistake! Instead of being an adult and fixing it, I made it worse for [poos] and giggles.
                                    //soph what the hell is that supposed to mean what have you done to forsake me now
                                    //4-25-23: lmao this probably doesnt work if the slice is extra long

        Instantiate(flag, new Vector3(relativeZero + slice.width + 10, 5, 0), Quaternion.identity);
        // flag.transform.position = new Vector3(relativeZero + slice.width + 10, 5, 0);

    }
    public void GenerateLevelEndpoint(LevelSlice[] slices, int iteration = 0)
    {

        //iteration is iteration; controls an x offset, basically
        var slice = slices[iteration];
        // get a slice to generate from

        //Generate blocks based off of
        //The slice image
        //The current level theme

        //2D for loop, going through a row, then proceeding down 

        Texture2D[] textures = slices.Select(x => x.texture).ToArray();
        int relativeZero = (from texture in textures.Take(iteration).ToList<Texture2D>()
                            select texture.width)
                            .Sum(); //I did this at 1 in the morning and made a mistake! Instead of being an adult and fixing it, I made it worse for [poos] and giggles.
                                    //soph what the hell is that supposed to mean what have you done to forsake me now
                                    //4-25-23: lmao this probably doesnt work if the slice is extra long

        Instantiate(flag, new Vector3(relativeZero + slice.texture.width + 10, 5, 0), Quaternion.identity);
        // flag.transform.position = new Vector3(relativeZero + slice.width + 10, 5, 0);

    }
    public void GenerateLevelChunk(Texture2D[] slices, int iteration = 0)
    {
        //"Don't repeat yourself" (DRY) is a principle of software development aimed at reducing repetition of software patterns,[1] replacing it with abstractions or using data normalization to avoid redundancy.
        //The DRY principle is stated as "Every piece of knowledge must have a single, unambiguous, authoritative representation within a system". The principle has been formulated by Andy Hunt and Dave Thomas in their book The Pragmatic Programmer.[2] They apply it quite broadly to include database schemas, test plans, the build system, even documentation.[3] When the DRY principle is applied successfully, a modification of any single element of a system does not require a change in other logically unrelated elements. Additionally, elements that are logically related all change predictably and uniformly, and are thus kept in sync. Besides using methods and subroutines in their code, Thomas and Hunt rely on code generators, automatic build systems, and scripting languages to observe the DRY principle across layers. 
        List<string> coolList = new List<string>()
        {
            "FFFFFFFF",
            "000000FF",
            "FF0000FF",
            "0000FFFF",
            "00FF00FF",
            "FF00FFFF",
            "009600FF",
            "FF9600FF",
            "000096FF",
            "009696FF",
            "00000000",
        };
        GameObject trapParent = new GameObject();
        //iteration is iteration; controls an x offset, basically
        var slice = slices[iteration];
        // get a slice to generate from

        //Generate blocks based off of
        //The slice image
        //The current level theme

        //2D for loop, going through a row, then proceeding down 

        int relativeZero = (from texture in slices.Take(iteration).ToList<Texture2D>()
                            select texture.width)
                            .Sum(); //I did this at 1 in the morning and made a mistake! Instead of being an adult and fixing it, I made it worse for [poos] and giggles.
                                    //soph what the hell is that supposed to mean what have you done to forsake me now
                                    //4-25-23: lmao this probably doesnt work if the slice is extra long


        //Beginning of xy loop

        Vector2 lastRecordedFossilCoord = Vector2.zero;

        for (int y = 0; y < slice.height; y++)
        {
            //presumes constant length
            for (int x = relativeZero; x < slice.width + relativeZero; x++)
            {
                Color colorPixel = slice.GetPixel(x - relativeZero, y);
                string colorString = ColorUtility.ToHtmlStringRGBA(colorPixel).Replace("94", "96");

                //so we dont filter everything, it is an expensive operation
                if (!(coolList.Contains(colorString)))
                {
                    //the hell algorithm
                    colorString = RoundHexRGBA(colorString);
                }

                //A "rounder" to recover corruption from god awful compression


                switch (colorString)
                {
                    case "FFFFFFFF": //white
                        Tilemap.SetTile(new Vector3Int(x, y, 0), null);
                        break;
                    case "000000FF": //black
                                     //Place a tile right here

                        Tilemap.SetTile(new Vector3Int(x, y, 0), currentLevelTile);

                        //So that we only generate fossils considerably below the surface
                        if (fossilGameObjects.Length > 0)
                        {
                            //check if the last recorded fossil is far enough for eligibility
                            if (Vector2.Distance(new Vector2(x, y), lastRecordedFossilCoord) > minFossilDistance) ;// Vector2.SqrMagnitude(new Vector2(x, y) - lastRecordedFossilCoord) > minFossilDistance
                            {
                                //generate random to maybe make a fossil    

                                System.Random random = new System.Random();
                                bool generatingFossil = random.Next(0, 100) > (100 - fossilChance);
                                if (generatingFossil)
                                {
                                    //instantiate
                                    Debug.Log("Creating fossil");
                                    lastRecordedFossilCoord = new Vector2(x, y);
                                    var silly = Instantiate(fossilGameObjects[UnityEngine.Random.Range(0, fossilGameObjects.Count())], new Vector3(x - 4, y - 35.1f, 0), Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
                                    silly.GetComponent<SpriteRenderer>().sortingOrder = 100;
                                }

                            }
                        }


                        break;
                    case "FF0000FF": //sheer red    //!Please note: Colors are on a scale of 0-1; every color will be equivalent to color/255!
                                     //Place an enemy
                                     // gameManager.SpawnEnemy(x, y - 15);
                        Instantiate(levelManager.GetEnemyDispenser().DispenseEnemy(), new Vector3(x, y - 15, 0), Quaternion.identity);
                        break;
                    case "0000FFFF": //sheer blue   //!Please note: Colors are on a scale of 0-1; every color will be equivalent to color/255!
                                     //Place the first 
                        SpawnTrap(x - 4, y - 34f, traps[0]);
                        break;
                    case "00FF00FF": //sheer green
                                     //Will place some type of shrubbery

                        if (UnityEngine.Random.Range(0, 100) > (100 - shrubChance))
                        {

                            int rando = UnityEngine.Random.Range(0, garnishGameObjects.Length - 1);

                            Instantiate(garnishGameObjects[rando], new Vector3(x - 4, y - 35.1f + garnishVerticalOffset, 0), Quaternion.identity);

                        }
                        break;
                    case "FF00FFFF": //porpol
                                     //Will place walls
                        bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelBGTile);


                        break;
                    case "FF0096FF": //porpol, shhhhhhhhhh
                                     //Will place walls
                        bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelBGTile);


                        break;
                    case "960096FF": //porpol, shhhhhhhhhh
                                     //Will place walls
                        bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelBGTile);


                        break;
                        //Will place walls and a regular tile
                        bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelBGTile);
                        Tilemap.SetTile(new Vector3Int(x, y, 0), currentLevelTile);

                        break;
                    case "009600FF": //Dark green
                                     //Will place details below the level
                        Tilemap.SetTile(new Vector3Int(x, y, 0), null);


                        break;
                    case "FF9600FF": //Orange
                                     //Will place a button, offset because tiles are gross
                        gameManager.SpawnButton(x - 4.5f, y - 34.5f, trapParent);

                        break;
                    case "009696FF": //cyan
                                     //Will place a trapped wall. This will be triggered by any orange (button) tiles
                        Debug.Log("its wallin' time");
                        gameManager.SpawnWall(x - 2.5f, y - 39, trapParent);


                        break;
                    default:
                        if (colorPixel.a == 0)//nothing there
                        {
                            Tilemap.SetTile(new Vector3Int(x, y, 0), null);
                        }
                        else
                        {
                            throw new System.ArgumentNullException($"Level Generation Malformation: Illegal Argument of {colorString} at texture {slice.name}");
                        }
                        break;

                }

            }
        }


    }
    public void GenerateLevelChunk(LevelSlice[] slices, int iteration = 0)
    {
        //"Don't repeat yourself" (DRY) is a principle of software development aimed at reducing repetition of software patterns,[1] replacing it with abstractions or using data normalization to avoid redundancy.
        //The DRY principle is stated as "Every piece of knowledge must have a single, unambiguous, authoritative representation within a system". The principle has been formulated by Andy Hunt and Dave Thomas in their book The Pragmatic Programmer.[2] They apply it quite broadly to include database schemas, test plans, the build system, even documentation.[3] When the DRY principle is applied successfully, a modification of any single element of a system does not require a change in other logically unrelated elements. Additionally, elements that are logically related all change predictably and uniformly, and are thus kept in sync. Besides using methods and subroutines in their code, Thomas and Hunt rely on code generators, automatic build systems, and scripting languages to observe the DRY principle across layers. 
        List<string> coolList = new List<string>()
        {
            "FFFFFFFF",
            "000000FF",
            "FF0000FF",
            "0000FFFF",
            "00FF00FF",
            "FF00FFFF",
            "009600FF",
            "FF9600FF",
            "000096FF",
            "00000000",
        };
        GameObject trapParent = new GameObject();
        //iteration is iteration; controls an x offset, basically
        var slice = slices[iteration];
        // get a slice to generate from

        //Generate blocks based off of
        //The slice image
        //The current level theme

        //2D for loop, going through a row, then proceeding down 
        //simple as dat
        Texture2D[] textures = slices.Select(x => x.texture).ToArray();
        Texture2D texture = textures[iteration];


        int relativeZero = (from tex in textures.Take(iteration).ToList<Texture2D>()
                            select tex.width)//haha ya got me
                            .Sum(); //I did this at 1 in the morning and made a mistake! Instead of being an adult and fixing it, I made it worse for [poos] and giggles.
                                    //soph what the hell is that supposed to mean what have you done to forsake me now
                                    //4-25-23: lmao this probably doesnt work if the slice is extra long


        //Beginning of xy loop

        Vector2 lastRecordedFossilCoord = Vector2.zero;

        for (int y = 0; y < texture.height; y++)
        {
            //presumes constant length
            for (int x = relativeZero; x < texture.width + relativeZero; x++)
            {
                Color colorPixel = texture.GetPixel(x - relativeZero, y);
                string colorString = ColorUtility.ToHtmlStringRGBA(colorPixel).Replace("94", "96");

                //so we dont filter everything, it is an expensive operation
                if (!(coolList.Contains(colorString)))
                {
                    //the hell algorithm
                    colorString = RoundHexRGBA(colorString);
                }

                //A "rounder" to recover corruption from god awful compression


                switch (colorString)
                {
                    case "FFFFFFFF": //white
                        Tilemap.SetTile(new Vector3Int(x, y, 0), null);
                        break;
                    case "000000FF": //black
                                     //Place a tile right here

                        Tilemap.SetTile(new Vector3Int(x, y, 0), currentLevelTile);

                        //So that we only generate fossils considerably below the surface
                        if (fossilGameObjects.Length > 0)
                        {
                            //check if the last recorded fossil is far enough for eligibility
                            if (Vector2.Distance(new Vector2(x, y), lastRecordedFossilCoord) > minFossilDistance) ;// Vector2.SqrMagnitude(new Vector2(x, y) - lastRecordedFossilCoord) > minFossilDistance
                            {
                                //generate random to maybe make a fossil    

                                System.Random random = new System.Random();
                                bool generatingFossil = random.Next(0, 100) > (100 - fossilChance);
                                if (generatingFossil)
                                {
                                    //instantiate
                                    Debug.Log("Creating fossil");
                                    lastRecordedFossilCoord = new Vector2(x, y);
                                    var silly = Instantiate(fossilGameObjects[UnityEngine.Random.Range(0, fossilGameObjects.Count())], new Vector3(x - 4, y - 35.1f, 0), Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
                                    silly.GetComponent<SpriteRenderer>().sortingOrder = 100;
                                }

                            }
                        }


                        break;
                    case "FF0000FF": //sheer red    //!Please note: Colors are on a scale of 0-1; every color will be equivalent to color/255!
                                     //Place an enemy
                                     // gameManager.SpawnEnemy(x, y - 15);
                        Instantiate(slice.enemy, new Vector3(x - 3, y - 15 - 16, 0), slice.enemy.transform.rotation);
                        break;
                    case "0000FFFF": //sheer blue   //!Please note: Colors are on a scale of 0-1; every color will be equivalent to color/255!
                                     //Place the first 
                        // SpawnTrap(x - 4, y - 34f, slice.traps[0]);
                        try
                        {
                        Instantiate(slice.traps[0], new Vector3(x-4, y-34),slice.traps[0].transform.rotation );
                            
                        }
                        catch (System.Exception)
                        {
                            Debug.Log(slice.name + " is the shitter");
                            throw;
                        }
                        break;
                    case "00FF00FF": //sheer green
                                     //Will place some type of shrubbery

                        if (UnityEngine.Random.Range(0, 100) > (100 - shrubChance))
                        {

                            int rando = UnityEngine.Random.Range(0, garnishGameObjects.Length - 1);

                            Instantiate(garnishGameObjects[rando], new Vector3(x - 4, y - 35.1f + garnishVerticalOffset, 0), Quaternion.identity);

                        }
                        break;
                    case "FF00FFFF": //porpol
                                     //Will place walls
                        bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelBGTile);


                        break;
                    case "960096FF": //porpol
                                     //Will place walls
                        bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelBGTile);


                        break;
                    case "FF0096FF": //porpol, shhhhhhhhhh
                                     //Will place walls
                        bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelBGTile);


                        break;
                        //Will place walls and a regular tile
                        bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelBGTile);
                        Tilemap.SetTile(new Vector3Int(x, y, 0), currentLevelTile);

                        break;
                    case "009600FF": //Dark green
                                     //Will place details below the level
                        Tilemap.SetTile(new Vector3Int(x, y, 0), null);


                        break;
                    case "FF9600FF": //Orange
                                     //Will place a button, offset because tiles are gross
                        gameManager.SpawnButton(x - 4.5f, y - 34.5f, trapParent);

                        break;
                    case "009696FF": //cyan
                                     //Will place a trapped wall. This will be triggered by any orange (button) tiles 
                        Debug.Log("its wallin' time");
                        var wallInstance = Instantiate(slice.traps[1], new Vector3(x - 2.5f, y - 39),slice.traps[1].transform.rotation );
                        // gameManager.SpawnWall(x - 2.5f, y - 39, trapParent);
                        wallInstance.transform.parent = trapParent.transform;


                        break;
                    default:
                        if (colorPixel.a == 0)//nothing there
                        {
                            Tilemap.SetTile(new Vector3Int(x, y, 0), null);
                        }
                        else
                        {
                            throw new System.ArgumentNullException($"Level Generation Malformation: Illegal Argument of {colorString} at texture {slice.name}");
                        }
                        break;

                }

            }
        }


    }

    ///<summary> Spawns a trap at the speciifed coordinate. </summary>
    private void SpawnTrap(float x, float y, GameObject trap)
    {
        Instantiate(trap, new Vector3(x, y, 0), Quaternion.identity);
    }



    public int TerribleRound(int input)
    {
        //  00, 96, ff
        int[] values = new int[] { 0, 150, 255 };
        int[] values2 = new int[] { 0, 150, 255 };

        List<int> outputs = values.ToList();
        for (int i = 0; i < outputs.Count; i++)
        {
            //this will create values that get lower the closer they are to the input.
            //YOOOOOOOOOOOOOO LISTS MAINTAIN ORDER??????????
            outputs[i] = Math.Abs((values[i]) - input);
        }
        //proably already does this without the "custom" key function, but whatever.

        //returning the element in values at the index of a parallel lists lowest value???
        //lmao as fuck bro
        return values[outputs.IndexOf(outputs.Min(x => x))];

        //fuck it stage 2

        //the possible outputs
        //wretched

    }
    ///<summary>
    ///Rounds a Hex RGBA value with a specific step. Don't put the goddamn hash in there, just six characters will do the job ok bud?
    ///</summary>
    public string RoundHexRGBA(string input)
    {
        string output = "";
        for (int i = 0; i < input.Length; i += 2)
        {
            //get the partiiton(eg "3A" or "FB" but not "AF" in #"3AFB53FF")
            var currentPartition = input.Substring(i, 2);
            // convert to int
            //i found this on the internet! Gee, colleges sure gotta get ahead of the game!
            int intVersion = Convert.ToInt32(currentPartition, 16);
            //round to a proper NORMAL value
            //TRIED to do an extension but Tim Corey broke my door open, then my ribcage.
            intVersion = TerribleRound(intVersion);
            //return to hex, put that shit into the output
            // output.Concat(intVersion.ToString());
            output += (intVersion.ToString("X2"));
        }

        return output;

    }
}
#region scrapped color bit
//    switch (ColorUtility.ToHtmlStringRGBA(color))
//             {
//                 case "RGBA(1.00, 1.00, 1.00, 1.00)": //white
//                     Tilemap.SetTile(new Vector3Int(x, y, 0), null);
//                     break;
//                 //whitespace(nothing)
//                 case "RGBA(0.00, 0.00, 0.00, 1.00)": //black
//                                                      //Place a tile right here

//                     Tilemap.SetTile(new Vector3Int(x, y, 0), currentLevelTile);


//                     break;
//                 case "RGBA(1.00, 0.00, 0.00, 1.00)": //sheer red    //!Please note: Colors are on a scale of 0-1; every color will be equivalent to color/255!
//                                                      //Place an enemy
//                     manager.SpawnEnemy(x, y-15);
//                     break;
//                  case "RGBA(0.00, 0.00, 1.00, 1.00)": //sheer blue   //!Please note: Colors are on a scale of 0-1; every color will be equivalent to color/255!
//                                                      //Place the first 
//                     SpawnTrap(x, y-15, traps[0]);
//                     break;
//                      case "RGBA(0.00, 1.00, 0.00, 1.00)": //sheer green
//                                                      //Will place some type of shrubbery

//                     int rando = UnityEngine.Random.Range(0, currentLevelgarnishTiles.Length - 1);

//                       bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelgarnishTiles[rando]);


//                     break;
//                      case "RGBA(1.00, 0.00, 1.00, 1.00)": //porpol
//                                                      //Will place walls
//                       bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelBGTile);


//                     break;
//                        case $"RGBA(1.00, 0.00, 1.00, 1.00)": //Dark green
//                                                      //Will place details below the level
//                       bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelBGTile);


//                     break;
//                 default:
//                     if (color.a == 0)//nothing there
//                     {
//                         Tilemap.SetTile(new Vector3Int(x, y, 0), null);
//                     }
//                     else
//                     {
//                         throw new System.ArgumentNullException($"Level Generation Malformation: Illegal Argument of {color.ToString("F2")}");
//                     }
//                     break;

#endregion
