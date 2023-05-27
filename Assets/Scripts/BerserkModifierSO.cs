using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Modifiers/Berserk", fileName = "Berserk SO")]
public class BerserkModifierSO : ModifierSO
{
 public void OnEnable()
    {
        // BerserkModifier modifier = new BerserkModifier();
    }
    // private BerserkModifier modifier = new BerserkModifier();
    public override string ToString()
    {
        return "Berserk!";
    }

    public override IModifier GetModifier()
    {
       return new BerserkModifier();
    }
  

}
