using UnityEngine;

internal class BeachBallPlayerState1 : IPlayerState
{
    private bool internalState;

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
        internalState = false;
        mod.bounceEnabled = false;

    }

    /// <summary> The effects when the toggle is "on," or active.</summary> 
    void IPlayerState.PositiveToggle()
    {
        //When the action is run, in this case, slamming the player to the ground.
        internalState = true;

        //bind to the sword
        mod.bounceEnabled = true;

    }
    public void Toggle()
    {
        internalState = mod.bounceEnabled = !mod.bounceEnabled;
    }
}