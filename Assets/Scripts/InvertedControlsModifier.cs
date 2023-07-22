using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedControlsModifier : UnityEngine.Object, IModifier, IMovementModifier
{
    private Shader normalShader;
    public RanoScript player;
    public IEnumerator ContinuousEffect(RanoScript RanoScript)
    {
       yield break;
    }

    public void OnStartEffect(RanoScript player)
    {
        player.controlInversion = -1;
        player.gameObject.GetComponentInChildren<SpriteRenderer>().material.shader = player.data.invertedShader;

    }
     public override string ToString()
    {
        return "Inverted Chaos!";
    }

    public void SetPlayer(RanoScript player)
    {
          this.player = player;
    }

    public void SetPermenantEffects(RanoScript player)
    {
        normalShader = player.gameObject.GetComponentInChildren<SpriteRenderer>().material.shader;
    }

    public Sprite GetIcon()
    {
    return (Sprite)Resources.Load("Mod Icons\\invertedcontrols");
    }

    public void OnNewModAdded(RanoScript rano)
    {
      return;
    }

    public void OnEndEffect(RanoScript player)
    {
       player.controlInversion = 1;
       player.gameObject.GetComponentInChildren<SpriteRenderer>().material.shader = normalShader;
    }
}
