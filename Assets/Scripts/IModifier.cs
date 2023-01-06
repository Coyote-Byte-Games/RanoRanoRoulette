using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface IModifier
{
    public void SetPlayerEffects(bettertestplayablescript player);
    public void OnStartEffect(bettertestplayablescript player);
    public IEnumerator ContinuousEffect(bettertestplayablescript bettertestplayablescript);
}