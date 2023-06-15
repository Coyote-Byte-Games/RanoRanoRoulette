using System;

public interface IPlayerAction : IModifierTrait
{
    void Run();
    bool OnCoolDown();
    public void DecrementCD();
    
    UnityEngine.Sprite GetIcon();
}