using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Modifiers/The World", fileName = "The World SO")]
public class TheWorldModifierSO : ModifierSO
{
 public void OnEnable()
    {
        // BerserkModifier modifier = new BerserkModifier();
    }
    // private BerserkModifier modifier = new BerserkModifier();
    public override string ToString()
    {
        return "The World!";
    }

    public override IModifier GetModifier()
    {
       return new TheWorldModifier();
    }
  

}
