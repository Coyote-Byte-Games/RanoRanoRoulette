using System.Collections;
using UnityEngine;

public class TheWorldModifier : UnityEngine.Object, IModifier, IAttackModifier, IMovementModifier, IJumpModifier
{
    public TheWorldPlayerAction1 stopTimeAction;
    public RanoScript player;
    //todo fix actions, array?

    public TheWorldModifier()
    {
        stopTimeAction = new TheWorldPlayerAction1(this);
    }
    public void OnNewModAdded(RanoScript rano)
    {
        return;
    }
    public void SetPlayer(RanoScript player)
    {
        this.player = player;
    }


    public IEnumerator ContinuousEffect(RanoScript player)
    {
        yield break;
    }

    public void OnEndEffect(RanoScript player)
    {
    player.RemoveAction(stopTimeAction);
    }

    public void OnStartEffect(RanoScript player)
    {
        player.AddAction(stopTimeAction);

    }

    public override string ToString()
    {
        return "The World!";
    }

    public void SetPermenantEffects(RanoScript player)
    {


    }

    public Sprite GetIcon()
    {
        //    return player.data.
        return (Sprite)Resources.Load("Mod Icons\\theworld");
    }
}