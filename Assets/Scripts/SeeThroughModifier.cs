using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughModifier : UnityEngine.Object, IModifier, IMovementModifier
{
    public RanoScript player;
    public void OnNewModAdded(RanoScript rano)
    {
          player.SetAlpha(.4f);
    }
    public IEnumerator ContinuousEffect(RanoScript rano)
    {
    yield break;
    }

    public void OnStartEffect(RanoScript player)
    {
        return;
    }
     public override string ToString()
    {
        return "See-Through!";
    }

    public void SetPlayer(RanoScript player)
    {
          this.player = player;
    }

    

    public Sprite GetIcon()
    {
       return (Sprite)Resources.Load("Mod Icons\\seethrough");
    }

    public void SetPlayerEffects(RanoScript player)
    {
          player.SetAlpha(.4f);
    }
}
