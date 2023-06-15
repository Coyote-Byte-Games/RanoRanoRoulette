using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedShoesModifier : UnityEngine.Object, IModifier, IMovementModifier
{
    public RanoScript player;
    [SerializeField]
    // public SFXManagerSO soundManager;
    public IEnumerator ContinuousEffect(RanoScript rano)
    {
        yield break; 
    }
public void OnNewModAdded(RanoScript rano)
    {
      return;
    }
    public void OnStartEffect(RanoScript player)
    {
     player.EnableSpikeBoots();
    }
    public override string ToString()
    {
        return "Spiked Boots!";
    }

    public void SetPlayer(RanoScript player)
    {
        this.player = player;
    }



    public Sprite GetIcon()
    {
        return (Sprite)Resources.Load("Mod Icons\\spikedboots");
    }

    public void SetPlayerEffects(RanoScript player)
    {
        return;
    }
}
