using Unity;
using UnityEngine;
public class BerserkPlayerAction1 : IPlayerAction
{
    private BerserkModifier mod;
    public BerserkPlayerAction1(BerserkModifier modSource)
    {
        this.mod = modSource;
    }
     void IPlayerAction.Run()
    {
        //When the action is run, in this case, flinging the player forward.

        //bind to the sword
        ((GameObject)mod.sword).GetComponent<Animator>().SetTrigger("Swing");
        mod.player.rb.AddForce(new Vector2(500,500));
    }

   
}