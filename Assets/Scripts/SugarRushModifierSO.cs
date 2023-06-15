using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "My Assets/Modifiers/SugarRush", fileName = "Sugar Rush SO")]
public class SugarRushModifierSO : ModifierSO
{
    public void OnEnable()
    {
        SugarRushModifier modifier = new SugarRushModifier();
    }
    private SugarRushModifier modifier = new SugarRushModifier();
    public override string ToString()
    {
        return modifier.ToString();
    }

    public override IModifier GetModifier()
    {
       return modifier;
    }
}
