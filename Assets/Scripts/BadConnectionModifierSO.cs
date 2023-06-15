using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "My Assets/Modifiers/BadConnection", fileName = "Bad Connection SO")]
public class BadConnectionModifierSO : ModifierSO
{
    public void OnEnable()
    {
       BadConnectionModifier modifier = new BadConnectionModifier();
    }
    private BadConnectionModifier modifier = new BadConnectionModifier();
    public override string ToString()
    {
        return modifier.ToString();
    }

    public override IModifier GetModifier()
    {
       return modifier;
    }
}
