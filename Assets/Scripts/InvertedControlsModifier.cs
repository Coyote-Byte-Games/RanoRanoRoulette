using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedControlsModifier : UnityEngine.Object, IModifier, IMovementModifier
{
    public bettertestplayablescript player;
    public IEnumerator ContinuousEffect(bettertestplayablescript bettertestplayablescript)
    {
       yield break;
    }

    public void OnStartEffect(bettertestplayablescript player)
    {
        player.controlInversion *= -1;
    }
     public override string ToString()
    {
        return "Inverted Chaos!";
    }

    public void SetPlayer(bettertestplayablescript player)
    {
          this.player = player;
    }

    public void SetPlayerEffects(bettertestplayablescript player)
    {
       return;
    }

    public Sprite GetIcon()
    {
       return player.GetComponent<SpriteRenderer>().sprite;
    }
}
