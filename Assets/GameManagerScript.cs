using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
  void AddMods(WheelScript script)
  {
    
  }






public GameData data;
public GameObject WheelPrefab;
public int modifierInterval;
  public GameObject player;
  // public GameObject wheel;
  private WheelScript wheelScript;
   float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {
     wheelScript = WheelPrefab.GetComponent<WheelScript>();
       LaunchNewModifier();
      
    }
    void Awake()
    {
      
      data.mods = Modifier.GenerateRandomMods(4);
      
    
    }

    // Update is called once per frame
    void Update()
    {
     
      //checking if we need a new modifier
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= modifierInterval)
        {
          timeElapsed = 0;
          LaunchNewModifier();

        }

    }
    //: Generates a new modifier. Includes the entire phase of spawning the wheel, spinning, choosing the modifier and ending.
     void LaunchNewModifier()
    {
      var wheelInstance = Instantiate(WheelPrefab, Vector3.zero, Quaternion.identity);
      Destroy(wheelInstance, 3);
      Modifier newMod = wheelScript.Launch();
      //:the modifier is null at this point
      player.GetComponent<bettertestplayablescript>().AddModifier(newMod);

      data.numOfMods--;
      // i am having a stroke
    
    
    }
}
//todo create new comments, docs, debug? explain, function?
