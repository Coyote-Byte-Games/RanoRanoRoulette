using System.Collections;
using UnityEngine;

public class  BerserkModifier : UnityEngine.Object, IModifier, IAttackModifier, IMovementModifier, IJumpModifier, ISpriteModifier
{
    //the sword
    public GameObject sword = null;
    public bettertestplayablescript player;
    //todo fix actions, array?
    private BerserkPlayerAction1 action;
    public BerserkModifier()
    {
        action = new BerserkPlayerAction1(this);
    }
    public void SetPlayer(bettertestplayablescript player)
    {
        this.player = player;
    }
  

    public IEnumerator ContinuousEffect(bettertestplayablescript player)
    {
        yield break;
    }

    public void OnEndEffect(bettertestplayablescript player)
    {

    }

    public void OnStartEffect(bettertestplayablescript player)
    {
    }

    public override string ToString()
    {
        return "Berserk!";
    }
    public void SetPlayerEffects(bettertestplayablescript player)
    {
        //adds berserk sword//!this may be an issue, as it relies on being a scriptable object
       sword = Instantiate( player.data.BerserkSword,(player.transform.position), Quaternion.identity );
       sword.transform.SetParent(player.transform); 
       sword.transform.Rotate(0, 0, -90);
       //adds the action of swinging the sword
       player.AddAction(action);
       player.rb.mass += 2f;
       player.jumpPower /= 20;
    }

    public Sprite GetIcon()
    {
       return player.data.BerserkSword.GetComponentInChildren<SpriteRenderer>().sprite;
    }
}