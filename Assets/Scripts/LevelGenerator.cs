using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
// [CreateAssetMenu(menuName ="My Assets/Level Generator")]
public class LevelGenerator
{


    public RuleTile currentLevelTile;
    public Tilemap Tilemap;
    
    public Texture2D[] sliceTextures;
    // Start is called before the first frame update
    public LevelGenerator(Tilemap tm, RuleTile tile, Texture2D[] textures)
    {
        this.currentLevelTile = tile;
        this.Tilemap = tm;
        this.sliceTextures = textures;

    }
   
    public void GenerateLevelChunks(int numOfChunks)
    {
        var slices = GenerateRandomChunkSlices(numOfChunks);
        for (int i = 0; i < numOfChunks; i++)
        {
            GenerateLevelChunk(slices, i); //I really don't know man
        }
    }
    public Texture2D[] GenerateRandomChunkSlices(int count)
    {
        Texture2D[] output = new Texture2D[count];
        for (int i = 0; i < count; i++)
        {   int index = UnityEngine.Random.Range(0,sliceTextures.Length);//!it says this is inclusive but also exlclusive ig
             Texture2D slice = sliceTextures[index];
            output[i] = slice;
        }
        return output;
    }
    
    public void GenerateLevelChunk(Texture2D[] slices, int iteration = 0 )
    {
        //iteration is iteration; controls an x offset, basically
        var slice = slices[iteration];
        // get a slice to generate from
        
        Debug.Log("tis the " + iteration + $" iteration, plus a length of {slice.width}");

        //Generate blocks based off of
            //The slice image
            //The current level theme

        //2D for loop, going through a row, then proceeding down 
        
        int relativeZero = (from texture in slices.Take(iteration).ToList<Texture2D>()
                            select texture.width)
                            .Sum(); //I did this at 1 in the morning and made a mistake! Instead of being an adult and fixing it, I made it worse for shits and giggles.

        for (int y = 0; y < slice.height; y++)
        {
            //presumes constant length
            for (int x = relativeZero; x < slice.width + relativeZero; x++)
            {
                 Color color = slice.GetPixel(x - relativeZero,y);
                //  RuleTile toSet;
                 switch (color.ToString("F2"))
                 {
                    case "RGBA(1.00, 1.00, 1.00, 1.00)": //white
                                              Tilemap.SetTile(new Vector3Int(x,y,0), null);
                        break;
                    //whitespace(nothing)
                    case "RGBA(0.00, 0.00, 0.00, 1.00)": //black
                    //Place a tile right here

                        Tilemap.SetTile(new Vector3Int(x,y,0), currentLevelTile);


                        break;
                        case "RGBA(0.00, 0.00, 0.00, 0.00)": //nothing there
                   

                         Tilemap.SetTile(new Vector3Int(x,y,0), null);
                     

                        break;
                    default:
                 
                        throw new System.Exception("WHOOOOOOPS WE BROKE IT umu [thats a real good one]");
                 }
                
            }
        }
       

    }



}
