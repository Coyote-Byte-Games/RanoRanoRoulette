using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedControlsModifier : UnityEngine.Object, IModifier, IMovementModifier
{
    public RanoScript player;
    public IEnumerator ContinuousEffect(RanoScript RanoScript)
    {

    //     foreach(var elem in player.gameObject.GetComponentsInChildren<SpriteRenderer>()){
    //      elem.material.shader = player.data.invertedShader;}
    //    yield return new WaitForSeconds(player.gameManager.modifierInterval + .01f);
       yield break;
    }

    public void OnStartEffect(RanoScript player)
    {
        player.controlInversion *= -1;
    }
     public override string ToString()
    {
        return "Inverted Chaos!";
    }

    public void SetPlayer(RanoScript player)
    {
          this.player = player;
    }

    public void SetPlayerEffects(RanoScript player)
    {
       player.gameObject.GetComponentInChildren<SpriteRenderer>().material.shader = player.data.invertedShader;
    }

    public Sprite GetIcon()
    {
       return player.GetComponent<SpriteRenderer>().sprite;
    }
}
