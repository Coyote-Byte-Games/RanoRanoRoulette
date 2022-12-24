using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
  void AddMods(WheelScript script)
  {
    
  }








  public GameObject player;
  public GameObject wheel;
  private WheelScript wheelScript;
    // Start is called before the first frame update
    void Start()
    {
     wheelScript = wheel.GetComponent<WheelScript>();
       LaunchNewModifier();
    }
    void Awake()
    {
      
      
    
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //: Generates a new modifier. Includes the entire phase of spawning the wheel, spinning, choosing the modifier and ending.
     void LaunchNewModifier()
    {

     
      Modifier newMod = wheelScript.Launch();
      //:the modifier is null at this point
      player.GetComponent<bettertestplayablescript>().AddModifier(newMod);
      // i am having a stroke
    
    
    }
}
//todo create new comments, docs, debug? explain, function?
