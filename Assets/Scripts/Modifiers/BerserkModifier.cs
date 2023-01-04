using System.Collections;
using UnityEngine;

public class  BerserkModifier : Modifier, IAttackModifier, IMovementModifier, IJumpModifier
{
    public override IEnumerator ContinuousEffect(bettertestplayablescript player)
    {
        for (;;)
        {
            
         Debug.Log("haha WHY");
         player.transform.GetComponent<SpriteRenderer>().color += new Color(0,1/225f,0,1);
         yield return null;
        }
    }

    public override void OnEndEffect(bettertestplayablescript player)
    {
              player.transform.GetComponent<SpriteRenderer>().color = Color.blue;

    }

    public override void OnStartEffect(bettertestplayablescript player)
    {
        player.transform.GetComponent<SpriteRenderer>().color = Color.red;
    }
}