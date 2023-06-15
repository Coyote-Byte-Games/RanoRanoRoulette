using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface IModifier
{
    //todo fix this garbage
    public void SetPlayerEffects(RanoScript player);
    public void OnStartEffect(RanoScript player);
    public IEnumerator ContinuousEffect(RanoScript RanoScript);
    public void SetPlayer(RanoScript player);
    public Sprite GetIcon();
    public void OnNewModAdded(RanoScript rano);
}