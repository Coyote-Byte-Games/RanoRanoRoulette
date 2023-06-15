using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Modifiers/SeeThrough", fileName = "See-Through SO")]
public class SeeThroughModifierSO : ModifierSO
{
    private SeeThroughModifier modifier = new SeeThroughModifier();

    public override IModifier GetModifier()
    {
       return modifier;
    }

    public void OnEnable()
    {
    }
  



}
