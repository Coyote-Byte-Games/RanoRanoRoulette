using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface IModifier
{
    //todo fix this garbage
    public void SetPlayerEffects(bettertestplayablescript player);
    public void OnStartEffect(bettertestplayablescript player);
    public IEnumerator ContinuousEffect(bettertestplayablescript bettertestplayablescript);
    public void SetPlayer(bettertestplayablescript player);
    public Sprite GetIcon();
}