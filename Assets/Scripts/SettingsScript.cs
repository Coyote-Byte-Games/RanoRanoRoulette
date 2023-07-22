using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{
    
    public GameConfig config;
    public ModifierManager modMan;
    public static int modInterval = 15;
    public static int chunkNum = 25;
    public LevelGenerator levelGen;
    // Start is called before the first frame update
    public void SetScrollSpeed(float speed)
    {
        config.scrollSpeed = ((int)(speed * 15));
    }
     public void SetZoom(float zoom)
    {
        config.zoom = ((int)((1 - zoom) * 50));
    }
    public void SetModifierInterval(string στρινγ)
    {
        int whyWereOutParametersInvented;
        bool success = int.TryParse(στρινγ, out whyWereOutParametersInvented);
        if (success)
        {
            modInterval = whyWereOutParametersInvented;
            modMan.modifierInterval = modInterval;
        }
        return;
    }
    public void SetChunkCount(string chunks)
    {
    int whyWereOutParametersInvented;
        bool success = int.TryParse(chunks, out whyWereOutParametersInvented);
        if (success)
        {
            chunkNum = whyWereOutParametersInvented;
            levelGen.numOfChunks = whyWereOutParametersInvented;
        }
        return;
    }
    public void SetSeed(string seedInput)
    {
        int whyWereOutParametersInvented;
        bool success = int.TryParse(seedInput, out whyWereOutParametersInvented);
        if (success)
        {
            GameConfig.SetSeed(whyWereOutParametersInvented);
        }
        else
        {
        }
        return;
    }
    public void RandomizeSeed()
    {

    }
}
