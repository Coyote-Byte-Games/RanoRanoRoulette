using  UnityEngine;
public class ShaderDispenser : ScriptableObject
{
    public Material[] shadersRaw;
public Material NoneMat;
public Material OutlineMat;
public Material InvertMat;
public Material BlurMat;
public Material OutlineBlurMat;
    
    
    public enum Shader 
    {
        INVERT = 2, //000000100, 1
        BLUR = 4, //000001000, 2

        WAVY = 8, //3
        OUTLINE = 16, //000000010, 4

        OUTLINEINVERT = OUTLINE + INVERT 
    }






    //     public List<KeyValuePair<string, int> shaderBinds
    // {
    //     new KeyValuePair("INVERT", 2)
    //     new KeyValuePair("BLUR", 4)
    //     new KeyValuePair("WAVY", 8)
    //     new KeyValuePair("OUTLINE", 1)6
    // }
    // List<UnityEngine.Shader> shaders = new List<UnityEngine.Shader>();
    
    public int returnBinaryIteratorThing(int input)
    {
        return (int)Mathf.Pow(2, input);
    }
    public int returnInverseBinaryIteratorThing(int input)
    {
     
        return (int)Mathf.Log(2, input);
    
    }

    public ShaderDispenser()
    {
        
    }
    // public ReturnShader(int enumCode)
    // {
    //     //Get the binary representation
    //     string binaryEnumCode = (System.Convert.ToString(enumCode, 2));
    //     //Seperate each of the numbers

    //     //convert those numbers into decimal

    //     //add them

    //     //turn those into a single shader
    // }
    // public Material GetMat(int combo)
    // {

    // }

    
    
}