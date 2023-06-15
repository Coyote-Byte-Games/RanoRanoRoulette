using Unity;
using UnityEngine;
public class BeachBallPlayerAction1 : IPlayerAction
{

    private BeachBallModifier mod;
    public BeachBallPlayerAction1(BeachBallModifier modSource)
    {
        this.mod = modSource;
    }

    public void DecrementCD()
    {
        return;
    }

    public Sprite GetIcon()
    {
        return mod.GetIcon();
    }

    public bool OnCoolDown()
    {
       return false;
    }

    void IPlayerAction.Run()
    {
        //When the action is run, in this case, slamming the player to the ground.

        //bind to the sword
        mod.player.rb.velocity = (new Vector2(0, -200));
        // mod.player.rb.sharedMaterial = null;

    }

   
}