using System;

public interface IPlayerAction : IModifierTrait
{
    void Run();
    UnityEngine.Sprite GetIcon();
}