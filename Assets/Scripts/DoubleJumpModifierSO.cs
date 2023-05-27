using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Modifiers/DoubleJump", fileName = "Double Jump SO")]
public class DoubleJumpModifierSO : ModifierSO
{
    private DoubleJumpModifier modifier = new DoubleJumpModifier();

    public override IModifier GetModifier()
    {
       return modifier;
    }

    public void OnEnable()
    {
        DoubleJumpModifier modifier = new DoubleJumpModifier();
    }
  



}
