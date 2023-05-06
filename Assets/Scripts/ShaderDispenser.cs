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
        NONE = 0,
        OUTLINE = 1,
        INVERT = 2,
        BLUR = 4,
        OUTLINEINVERT = OUTLINE + INVERT
    }
    // List<UnityEngine.Shader> shaders = new List<UnityEngine.Shader>();
    

    public ShaderDispenser()
    {
        
    }
    // public Material GetMat(int combo)
    // {

    // }

    
    
}