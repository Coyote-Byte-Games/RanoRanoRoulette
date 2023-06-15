using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Modifiers/Adoption", fileName = "adoption")]
public class AdoptionSO : ModifierSO
{
    public void OnEnable()
    {
        AdoptionModifier modifier = new AdoptionModifier();
    }
    private AdoptionModifier modifier = new AdoptionModifier();
    public override string ToString()
    {
        return modifier.ToString();
    }

    public override IModifier GetModifier()
    {
       return modifier;
    }
}
