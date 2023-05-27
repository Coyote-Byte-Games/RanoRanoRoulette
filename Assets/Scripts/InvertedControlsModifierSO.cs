using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Modifiers/InvertedControls", fileName = "Inverted Controls SO")]
public class InvertedControlsModifierSO : ModifierSO
{
    private InvertedControlsModifier modifier;

    public override IModifier GetModifier()
    {
       return modifier;
    }

    public void OnEnable()
    {
        InvertedControlsModifier modifier = new InvertedControlsModifier();
    }
  



}
