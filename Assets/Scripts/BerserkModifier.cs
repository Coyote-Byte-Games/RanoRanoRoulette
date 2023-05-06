using System.Collections;
using UnityEngine;

public class  BerserkModifier : UnityEngine.Object, IModifier, IAttackModifier, IMovementModifier, IJumpModifier
{
    //the sword
    public GameObject sword = null;
    public RanoScript player;
    //todo fix actions, array?
    private BerserkPlayerAction1 action;
    public BerserkModifier()
    {
        action = new BerserkPlayerAction1(this);
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

    }

    public void OnStartEffect(RanoScript player)
    {
    }

    public override string ToString()
    {
        return "Berserk!";
    }
    public void SetPlayerEffects(RanoScript player)
    {
        //adds berserk sword//!this may be an issue, as it relies on being a scriptable object
       sword = Instantiate( player.data.BerserkSword,(player.transform.position), Quaternion.identity );
       sword.transform.SetParent(player.transform.GetChild(1)); 
    //    sword.transform.Rotate(0, 0, -90);
       //adds the action of swinging the sword
       player.AddAction(action);
       player.rb.mass += .5f;
       player.jumpPower /= 20;
    }
    public void SetRotation(int arg)
     {
        sword.transform.Rotate(0,0,arg - sword.GetComponent<Rigidbody2D>().rotation );
     }

    public Sprite GetIcon()
    {
    //    return player.data.
    return (Sprite)Resources.Load("Mod Icons\\berserk");
    }
}