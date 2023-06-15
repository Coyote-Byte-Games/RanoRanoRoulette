using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DoubleJumpModifier : UnityEngine.Object, IModifier, IMovementModifier, IJumpModifier, IBorderlineFlight
{
    private RanoScript player;
    public GameObject wings = null;

    public IEnumerator ContinuousEffect(RanoScript RanoScript)
    {
        yield break;
    }

  public override string ToString()
    {
        return "Double Jump!";
    }

    public Sprite GetIcon()
    {
      return (Sprite)Resources.Load("Mod Icons\\doublejump");

    }

    
public void OnNewModAdded(RanoScript rano)
    {
      return;
    }
    public void OnStartEffect(RanoScript player)
    {
    }

    public void SetPlayer(RanoScript player)
    {
       this.player = player;
    }

    public void SetPlayerEffects(RanoScript player)
    {
       player.maxJumps += 1;
      //  player.animator = GameData.ranoAnim;
       wings = Instantiate( player.data.DJWings,(player.transform.position), Quaternion.identity );
       wings.transform.SetParent(player.transform.GetChild(1)); 
    }
}