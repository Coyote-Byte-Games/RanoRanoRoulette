using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "My Assets/Modifiers/SpikedShoes", fileName = "Spiked Shoes SO")]
public class SpikedShoesModifierSO : ModifierSO
{
    public void OnEnable()
    {
        SpikedShoesModifier modifier = new SpikedShoesModifier();
    }
    private SpikedShoesModifier modifier = new SpikedShoesModifier();
    public override string ToString()
    {
        return modifier.ToString();
    }

    public override IModifier GetModifier()
    {
       return modifier;
    }
}
