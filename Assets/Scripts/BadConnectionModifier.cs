using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadConnectionModifier : UnityEngine.Object, IModifier, IMovementModifier
{
    public RanoScript player;
    [SerializeField]
    // public SFXManagerSO soundManager;
    public IEnumerator ContinuousEffect(RanoScript rano)
    {

         yield return new WaitForSeconds(4f);
        for (; ; )
        { 

            Vector2 priorPosition = rano.rb.position;
            yield return new WaitForSeconds(1f);
            
            rano.entityBase.soundManager.PlayClip(SFXManagerSO.Sound.AOL);
            rano.rb.position = priorPosition;
            yield return new WaitForSeconds(8f);
           
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
        return "Bad Connection!";
    }

    public void SetPlayer(RanoScript player)
    {
        this.player = player;
    }



    public Sprite GetIcon()
    {
        return (Sprite)Resources.Load("Mod Icons\\badconnection");
    }

    public void SetPermenantEffects(RanoScript player)
    {
        return;
    }

    public void OnEndEffect(RanoScript player)
    {
        return;
    }
}
