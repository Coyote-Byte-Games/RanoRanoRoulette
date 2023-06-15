using System.Collections;
using UnityEngine;

public class  BerserkModifier : UnityEngine.Object, IModifier, IAttackModifier, IMovementModifier, IJumpModifier
{
    //the sword
    

    public GameObject sword = null;
    public GameObject sheathedSword = null;
    public RanoScript player;
    //todo fix actions, array?
    private BerserkPlayerAction1 action;
    public BerserkModifier()
    {
        action = new BerserkPlayerAction1(this);
    }public void OnNewModAdded(RanoScript rano)
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

    }

    public void OnStartEffect(RanoScript player)
    {
    }

    public override string ToString()
    {
        return "Berserk!";
    }
    public void SheatheSword()
{
sword.GetComponentInChildren<SpriteRenderer>().enabled = false;
    sword.GetComponent<BoxCollider2D>().enabled = false;

sheathedSword.SetActive( true);
}
public void DrawSword()
{
    sword.GetComponentInChildren<SpriteRenderer>().enabled = true;
    sword.GetComponent<BoxCollider2D>().enabled = true;

    sheathedSword.SetActive( false);
}

    public void SetPlayerEffects(RanoScript player)
    {
        //adds berserk sword//!this may be an issue, as it relies on being a scriptable object
       sword = Instantiate( player.data.BerserkSword,(player.transform.position), Quaternion.Euler(0, 0, -217.39f) );
    //    sword.SetActive(false);
       sheathedSword = Instantiate( player.data.BerserkSwordSheathed,(player.transform.position), Quaternion.identity );
    //    sheathedSword.SetActive(true);
       sword.transform.SetParent(player.transform); 
       sheathedSword.transform.SetParent(player.transform); 
    //    sword.transform.Rotate(0, 0, -90);
       //adds the action of swinging the sword
       player.AddAction(action);
       player.rb.mass += .25f;
       player.jumpPower /= 20;
       SheatheSword();
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