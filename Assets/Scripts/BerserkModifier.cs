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
        for (;;)
        {
            
         
         player.transform.GetComponent<SpriteRenderer>().color += new Color(0,1/225f,0,1);
         yield return null;
        }
    }

    public void OnEndEffect(bettertestplayablescript player)
    {
              player.transform.GetComponent<SpriteRenderer>().color = Color.blue;

    }

    public void OnStartEffect(bettertestplayablescript player)
    {
        player.transform.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public override string ToString()
    {
        return "Berserk!";
    }
    public void SetPlayerEffects(bettertestplayablescript player)
    {
        //adds berserk sword//!this may be an issue, as it relies on being a scriptable object
       sword = Instantiate( player.data.BerserkSword);
       sword.transform.SetParent(player.transform); 
       sword.transform.Rotate(0, 0, -90);
       //adds the action of swinging the sword
       player.AddAction(action);
    }
}