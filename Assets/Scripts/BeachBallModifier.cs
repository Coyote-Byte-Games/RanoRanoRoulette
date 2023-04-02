using System.Collections;
using UnityEngine;
public class BeachBallModifier : UnityEngine.Object, IModifier, IMovementModifier, IJumpModifier, ISpriteModifier
{
    public bettertestplayablescript player;
    private BeachBallPlayerAction1 slamAction;
     private BeachBallPlayerState1 bounceToggle;
     public bool bounceEnabled; 
    public IEnumerator ContinuousEffect(bettertestplayablescript player)
    {
       for(;;)
       {
        if (player.Grounded() && player.rb.sharedMaterial == null && bounceEnabled) //if the player isnt bouncy but grounded
        {
            yield return new WaitForSeconds(.2f);
            player.rb.sharedMaterial =  player.data.bouncyMat;
           
        }
        if (!bounceEnabled)
        {
             player.rb.sharedMaterial = null;
        }

        player.transform.Rotate(0, 0, -0.02f * player.GetVel());
         yield return null;

       }
    }

    public Sprite GetIcon()
    {
            //    return player.data.FatRanoIcon;
            return (Sprite)Resources.Load("Mod Icons\\BeachBall");
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
        bounceToggle = new BeachBallPlayerState1(this);
        player.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        player.gameObject.GetComponent<CircleCollider2D>().enabled =true;

        

        player.jumpRadius += 2;
        // player.rb.mass /= 2;
        
        player.rb.constraints = UnityEngine.RigidbodyConstraints2D.None;
        player.UpdateSprite(player.data.FatRano); 
        player.rb.sharedMaterial = player.data.bouncyMat;
        player.GetCollider().sharedMaterial = player.data.bouncyMat;
        player.GetComponent<CircleCollider2D>().radius = 2f;
        
        player.GetComponent<CircleCollider2D>().offset += new Vector2(0,-1.5f);


        player.AddAction(slamAction);
        Debug.Log("ADDING THE STATE NOW");
        player.AddState(bounceToggle);
        
        
    }
    public override string ToString()
    {
        return "Chub!";
    }
}