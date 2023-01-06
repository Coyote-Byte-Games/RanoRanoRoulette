using System.Collections;
using UnityEngine;

public class  BerserkModifier : ScriptableObject, IAttackModifier, IMovementModifier, IJumpModifier, ISpriteModifier
{
    public IEnumerator ContinuousEffect(bettertestplayablescript player)
    {
        for (;;)
        {
            
         Debug.Log("haha WHY");
         player.transform.GetComponent<SpriteRenderer>().color += new Color(0,1/225f,0,1);
         yield return null;
        }
    }

    public void OnEndEffect(bettertestplayablescript player)
    {
              player.transform.GetComponent<SpriteRenderer>().color = Color.blue;

    }

    public void OnStartEffect(bettertestplayablescript player)
    {
        player.transform.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void SetPlayerEffects(bettertestplayablescript player)
    {
        //adds berserk sword//!this may be an issue, as it relies on being a scriptable object
       var sword = Instantiate( player.data.BerserkSword);
       sword.transform.SetParent(player.transform); 
    }
}