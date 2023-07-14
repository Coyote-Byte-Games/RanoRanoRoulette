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
    public EnemyDispenser enemyDispenser;
    public LevelGenerator generator;
    public Sprite bgImage;
    public Vector2 bgScale;
    public Color bgColor;
    
    public void GroomBackground(GameObject arg)
    {
        arg.GetComponent<SpriteRenderer>().sprite = bgImage;
        arg.GetComponent<SpriteRenderer>().color = bgColor;
        arg.transform.localScale = bgScale;

    }
    [System.Obsolete ("Use the New version using Level Slices to manage enemies and traps, if possible.")]
    public EnemyDispenser GetEnemyDispenser()
    {
        return enemyDispenser;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
