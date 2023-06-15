using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/GameConfig")]
public class GameConfig : ScriptableObject
{
    public int zoom = 25;
    public int scrollSpeed;
    private static int seed = -2;
    public static int GetSeed()
    {
        if (!(seed > 0))
        {
            seed = new System.Random().Next();
        }
        return seed;
    }

    public static void SetSeed(int value)
    {
        seed = value;
    }

    // public bool usingMouse;

}
