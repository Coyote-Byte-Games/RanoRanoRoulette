using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Responsible for all enemy creation and behaviour logic. Ideally, it can create an enemy based off of requirements given to it.
///</summary>
[CreateAssetMenu(menuName ="My Assets/Levels/Enemy Dispenser")]
public class EnemyDispenser : ScriptableObject
{

    public GameObject[] enemies;
    public GameObject DispenseEnemy()
    {
        return enemies[0];
    }
}
