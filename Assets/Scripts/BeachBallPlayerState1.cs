using UnityEngine;

internal class BeachBallPlayerState1 : IPlayerState
{
    private BeachBallModifier mod;
     public BeachBallPlayerState1(BeachBallModifier modSource)
    {
        this.mod = modSource;
    }

    public Sprite GetIcon()
    {
        return mod.GetIcon();
    }

    public void NegativeToggle()
    {
        mod.bounceEnabled = false;

    }

    /// <summary> The effects when the toggle is "on," or active.</summary> 
    void IPlayerState.PositiveToggle()
    {
        //When the action is run, in this case, slamming the player to the ground.

        //bind to the sword
        mod.bounceEnabled = true;

    }
}