using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
// [CreateAssetMenu(menuName ="My Assets/Level Generator")]
public class LevelGenerator : UnityEngine.Object
{



/*
0   0   0   = Ground 

255 0   0   = Enemy[0]

0   0   255 = Flame Pillar
0   1   255 = Flame pillar extrusion
*/




    public RuleTile currentLevelTile;
    public Tile currentLevelBGTile;
    public Tile[] currentLevelgarnishTiles;
    public Tilemap Tilemap;
    public Tilemap bgTilemap;
    public Texture2D[] sliceTextures;
    public float[] sliceTexProbabliities;
    public GameObject[] traps;
    public GameObject flag;

    // [SerializeField]
    // private GameObject trap1;

    public GameManagerScript manager;

        public LevelGenerator(GameManagerScript gameManager, Tilemap tm, Tilemap bgtm, RuleTile tile, Tile bgtile, Tile[] garnish, Texture2D[] textures, GameObject[] traps)
    {
        this.manager = gameManager;
        this.currentLevelTile = tile;
        this.currentLevelBGTile = bgtile;
        this.currentLevelgarnishTiles = garnish;
        this.Tilemap = tm;
        this.bgTilemap = bgtm;

        this.sliceTextures = textures;
        this.traps = traps;

    }

    public void GenerateLevelChunks(int numOfChunks)
    {
        var slices = GenerateRandomChunkSlices(numOfChunks);
        for (int i = 0; i < numOfChunks; i++)
        {
            GenerateLevelChunk(slices, i); //I really don't know man
        }
    }
    public void GenerateLevelChunksWithEndpoint(int numOfChunks)
    {
        var slices = GenerateRandomChunkSlices(numOfChunks);
        int i = 0;
        while ( i < numOfChunks) 
        {
            GenerateLevelChunk(slices,  i); //I really don't know man

            i++;
        }
        GenerateLevelEndpoint(slices, i - 1);

        
    }
    /// <summary>  
    /// <c>dsfd</c>
    /// </summary>
    
    public Texture2D[] GenerateRandomChunkSlices(int count)
    {
        Texture2D[] output = new Texture2D[count];
        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, sliceTextures.Length);//!it says this is inclusive but also exlclusive ig
            Texture2D slice = sliceTextures[index];
            output[i] = slice;
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

            // Instantiate(flag, new Vector3(relativeZero + slice.width + 10, 5, 0), Quaternion.identity);
            flag.transform.position = new Vector3(relativeZero + slice.width + 10, 5, 0);        

}
    public void GenerateLevelChunk(Texture2D[] slices, int iteration = 0)
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

     

        for (int y = 0; y < slice.height; y++)
        {
            //presumes constant length
            for (int x = relativeZero; x < slice.width + relativeZero; x++)
            {
                Color color = slice.GetPixel(x - relativeZero, y);
                //  RuleTile toSet;
                switch (color.ToString("F2"))
                {
                    case "RGBA(1.00, 1.00, 1.00, 1.00)": //white
                        Tilemap.SetTile(new Vector3Int(x, y, 0), null);
                        break;
                    //whitespace(nothing)
                    case "RGBA(0.00, 0.00, 0.00, 1.00)": //black
                                                         //Place a tile right here

                        Tilemap.SetTile(new Vector3Int(x, y, 0), currentLevelTile);


                        break;
                    case "RGBA(1.00, 0.00, 0.00, 1.00)": //sheer red    //!Please note: Colors are on a scale of 0-1; every color will be equivalent to color/255!
                                                         //Place an enemy
                        manager.SpawnEnemy(x, y-15);
                        break;
                     case "RGBA(0.00, 0.00, 1.00, 1.00)": //sheer blue   //!Please note: Colors are on a scale of 0-1; every color will be equivalent to color/255!
                                                         //Place the first 
                        SpawnTrap(x, y-15, traps[0]);
                        break;
                         case "RGBA(0.00, 1.00, 0.00, 1.00)": //sheer green
                                                         //Will place some type of shrubbery

                        int rando = UnityEngine.Random.Range(0, currentLevelgarnishTiles.Length - 1);

                          bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelgarnishTiles[rando]);


                        break;
                         case "RGBA(1.00, 0.00, 1.00, 1.00)": //porpol
                                                         //Will place walls
                          bgTilemap.SetTile(new Vector3Int(x, y, 0), currentLevelBGTile);


                        break;
                    default:
                        if (color.a == 0)//nothing there
                        {
                            Tilemap.SetTile(new Vector3Int(x, y, 0), null);
                        }
                        else
                        {
                            throw new System.ArgumentNullException($"Level Generation Malformation: Illegal Argument of {color.ToString("F2")}");
                        }
                        break;

                }

            }
        }


    }

///<summary> Spawns a trap at the speciifed coordinate. </summary>
    private void SpawnTrap(int x, int y, GameObject trap)
    {
      Instantiate(trap, new Vector3(x,y, 0), Quaternion.identity);
    }
    private void SpawnTrap(int x, int y)
    {
      int selector = UnityEngine.Random.Range(0, traps.Length);
    }

}
