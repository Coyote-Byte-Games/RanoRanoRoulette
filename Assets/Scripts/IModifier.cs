using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface IModifier
{
    //todo fix this garbage
    ///<summary>
    ///The initial, permenant effects of a modiifer that aren't canelled by OnEndEffect().
    ///</summary>
    public void SetPermenantEffects(RanoScript player);
    ///<summary>
    ///The effects that are added for use when the modifier is enabled. All effects here should be removed in OnEndEffect(). For Permenant effects, SetPermenantEffects().
    ///</summary>
    public void OnStartEffect(RanoScript player);
    ///<summary>
    ///Removes the effects from OnStartEffect, but not SetPermenantEffects().
    ///</summary>
    public void OnEndEffect(RanoScript player);
    public IEnumerator ContinuousEffect(RanoScript RanoScript);
    public void SetPlayer(RanoScript player);
    public Sprite GetIcon();
    public void OnNewModAdded(RanoScript rano);
}