using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModifierSO : ScriptableObject
{
   public abstract IModifier GetModifier();

}
