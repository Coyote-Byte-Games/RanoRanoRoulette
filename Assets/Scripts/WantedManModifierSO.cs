using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Modifiers/WantedMan")]
public class WantedManModifierSO : ModifierSO
{
    public void OnEnable()
    {
        WantedManModifier modifier = new WantedManModifier();
    }
    private WantedManModifier modifier = new WantedManModifier();
    public override string ToString()
    {
        return modifier.ToString();
    }
    public override IModifier GetModifier()
    {
        return modifier;
    }


}
