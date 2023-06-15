using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarRushModifier : UnityEngine.Object, IModifier, IMovementModifier
{
    public RanoScript player;
    
    [SerializeField]
    // public SFXManagerSO soundManager;
    public IEnumerator ContinuousEffect(RanoScript rano)
    {

        for (; ; )
        {
            
            rano.speedModifier *= 2f;
            rano.CreatePopup("Speed Up", 1);

            yield return new WaitForSeconds(10);
            rano.speedModifier *= (.75f) / 2f;
  
            rano.CreatePopup("Speed Down", 0);
            yield return new WaitForSeconds(10);
            rano.speedModifier /= .75f;

        }

    }
    public void OnNewModAdded(RanoScript rano)
    {
        return;
    }
    public void OnStartEffect(RanoScript player)
    {
        return;
    }
    public override string ToString()
    {
        return "Sugar Rush!";
    }

    public void SetPlayer(RanoScript player)
    {
        this.player = player;
    }



    public Sprite GetIcon()
    {
        return (Sprite)Resources.Load("Mod Icons\\sugarrush");
    }

    public void SetPlayerEffects(RanoScript player)
    {
        return;
    }
}
