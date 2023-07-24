using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="My Assets/Levels/Level Manager")]
public class LevelManagerScript : ScriptableObject
{
    public void Awake()
    {
        generator.levelManager = this;
    }
    public LevelGeneratorV2 generator;
    public Sprite bgImage;
    public Vector2 bgScale;
    public Color bgColor;
    
    public void GroomBackground(GameObject arg)
    {
        arg.GetComponent<SpriteRenderer>().sprite = bgImage;
        arg.GetComponent<SpriteRenderer>().color = bgColor;
        arg.transform.localScale = bgScale;

    }
}
