using System;

public interface IPlayerState : IModifierTrait
{


    void PositiveToggle();
    void NegativeToggle();
    void Toggle();
    UnityEngine.Sprite GetIcon();
    bool GetToggleState();
}