using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarRushModifier : UnityEngine.Object, IModifier, IMovementModifier
{
    public RanoScript player;
    
    public IEnumerator ContinuousEffect(RanoScript rano)
    {

        for (; ; )
        {
            rano.speedModifiers[name] = 2;
            rano.GetComponent<BuffableBehaviour>().CreatePopup("Speed Up", BuffableBehaviour.BuffIcon.GenericBuff);

            yield return new WaitForSeconds(10);
            rano.speedModifiers[name] = .75f;

  
            rano.GetComponent<BuffableBehaviour>().CreatePopup("Speed Down", BuffableBehaviour.BuffIcon.GenericDebuff);
            yield return new WaitForSeconds(10);
            rano.speedModifiers[name] = .75f;

        }

    }
    public void OnNewModAdded(RanoScript rano)
    {
        return;
    }
    public void OnStartEffect(RanoScript player)
    {
        
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

    public void SetPermenantEffects(RanoScript player)
    {
        //establish the value in speedModifiers
        player.speedModifiers.Add(this.ToString(), 1);
    }

    public void OnEndEffect(RanoScript player)
    {
        return;
    }
}
