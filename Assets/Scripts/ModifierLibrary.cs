using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierLibrary
{
#region Methods
//exists just to be able to pass in nothing
    public static void DoNada(IPlayerScript playah)
    {
        
    }
#region Kaboom
//: Creates an explosion at the player's coordinates, and adds a random force.
    
    public static void KaboomOnStart(IPlayerScript playah)
    {

    }
#endregion Kaboom

#endregion
#region Objects
      Modifier Kaboom = new Modifier(KaboomOnStart, null, null);
#endregion

}
