using System.Collections;
using UnityEngine;
public class BeachBallModifier : UnityEngine.Object, IModifier, IMovementModifier, IJumpModifier, ISpriteModifier
{
    public bettertestplayablescript player;
    private BeachBallPlayerAction1 slamAction;
    public IEnumerator ContinuousEffect(bettertestplayablescript player)
    {
       for(;;)
       {
        if (player.Grounded() && player.rb.sharedMaterial == null) //if the player isnt bouncy but grounded
        {
            yield return new WaitForSeconds(.2f);
            player.rb.sharedMaterial =  player.data.bouncyMat;
           
        }

        player.transform.Rotate(0, 0, -0.02f * player.GetVel());
         yield return null;
       }
    }

    public void OnEndEffect(bettertestplayablescript player)
    {
        player.rb.constraints = UnityEngine.RigidbodyConstraints2D.FreezeRotation;
        player.gameObject.transform.localScale.Set(1,1,0);
        player.rb.sharedMaterial = null;
        player.GetCollider().sharedMaterial = null;
        
    }

    public void OnStartEffect(bettertestplayablescript player)
    {
       
    }

    public void SetPlayer(bettertestplayablescript player)
    {
         this.player = player;
    }

    public void SetPlayerEffects(bettertestplayablescript player)
    {
        slamAction = new BeachBallPlayerAction1(this);
        player.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        player.gameObject.GetComponent<CircleCollider2D>().enabled =true;

        

        player.jumpRadius += 1f;
        player.rb.mass /= 2;
        player.rb.constraints = UnityEngine.RigidbodyConstraints2D.None;
        player.gameObject.transform.localScale += new UnityEngine.Vector3(3,1,0);
        player.rb.sharedMaterial = player.data.bouncyMat;
        player.GetCollider().sharedMaterial = player.data.bouncyMat;
        player.GetComponent<CircleCollider2D>().radius = 0.7f;

        player.AddAction(slamAction);
        
        
    }
    public override string ToString()
    {
        return "Chub!";
    }
}