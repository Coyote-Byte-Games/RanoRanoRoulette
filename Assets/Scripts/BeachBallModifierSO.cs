using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Modifiers/BeachBall", fileName = "Beach Ball SO")]
public class BeachBallModifierSO : ModifierSO
{
    public void OnEnable()
    {
        BeachBallModifier modifier = new BeachBallModifier();
    }
    private BeachBallModifier modifier = new BeachBallModifier();
    public override string ToString()
    {
        return modifier.ToString();
    }

    public override IModifier GetModifier()
    {
       return modifier;
    }
}
