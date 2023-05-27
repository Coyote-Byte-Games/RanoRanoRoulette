using System.Collections;
using UnityEngine;
public class BeachBallModifier : UnityEngine.Object, IModifier, IMovementModifier, IJumpModifier, IAnimationOverrideModifier, IFreeRotationModifier, IBorderlineFlight
{
    public RanoScript player;
    private BeachBallPlayerAction1 slamAction;
     private BeachBallPlayerState1 bounceToggle;
     public bool bounceEnabled; 
     private GameObject slamEffect;
    public float crashThreshold;

    
    public IEnumerator ContinuousEffect(RanoScript player)
    {
       for(;;)
       {
        if (player.Grounded() && player.rb.sharedMaterial == player.data.normalMat && bounceEnabled) //if the player isnt bouncy but grounded
        {
            yield return new WaitForSeconds(.2f);
            player.rb.sharedMaterial =  player.data.bouncyMat;
            // player.rb.sharedMaterial.bounciness /= (player.rb.mass/2);
           
        }
        if (!bounceEnabled)
        {
            //lazy fix
             player.rb.sharedMaterial = player.data.normalMat;
             
        }
        //player.transform.Rotate(0, 0, -0.02f * player.GetVel());
    
        player.transform.GetChild(1).transform.Rotate(0,0,-.14f * player.GetVel(), Space.Self);

        HandleImpactFX();







         yield return null;

       }
    }

    public Sprite GetIcon()
    {
            //    return player.data.FatRanoIcon;
            // Debug.Log("haha fortnite  " + (Sprite)Resources.Load("Mod Icons\\BeachBall") is null);
            return (Sprite)Resources.Load("Mod Icons\\BeachBall");
    }

    public void OnEndEffect(RanoScript player)
    {
        player.rb.constraints = UnityEngine.RigidbodyConstraints2D.FreezeRotation;
        player.gameObject.transform.localScale.Set(1,1,0);
        player.rb.sharedMaterial = null;
        player.GetCollider().sharedMaterial = null;
        
    }

private Vector2 oldDirection;
//to invoke repeating?
    private void HandleImpactFX()
    {
        //only want this to happen if the bounce is enabled
        if (!bounceEnabled)
        {
            return;
        }
        if (oldDirection == null)
        {
            oldDirection =  player.rb.velocity;
            return;
        }

        float newSpeed = player.rb.velocity.sqrMagnitude;

        if (oldDirection.sqrMagnitude - crashThreshold * crashThreshold >= newSpeed)
        {
            //do the crash

            //Get the tan(pointer towards the direction of the body), then turn it to degrees.
            // + 180 degrees to reverse. Since 0 deg on transform means its facing down, but 90 deg collision would be facing right, we need to remove 90 degrees to change the behaviour
            //and then i screwed with the math via guess and check
            var direction =  Mathf.Atan2(oldDirection.y, oldDirection.x)*Mathf.Rad2Deg -240; 
            
            Quaternion rotation = Quaternion.Euler(0,0,direction);
            Instantiate(slamEffect, player.groundCheck.position, rotation);

        }

    oldDirection = player.rb.velocity;
    }

    public void OnStartEffect(RanoScript player)
    {
       
    }

    public void SetPlayer(RanoScript player)
    {
         this.player = player;
         crashThreshold = player.crashThreshold;
         slamEffect = player.jumpEffect;
         slamEffect.transform.lossyScale.Set(2f,2f,0f);

    }

    public void SetPlayerEffects(RanoScript player)
    {
        slamAction = new BeachBallPlayerAction1(this);
        bounceToggle = new BeachBallPlayerState1(this);
        player.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        player.gameObject.GetComponent<CircleCollider2D>().enabled =true;

        

        player.jumpRadius += 2;
        player.rb.mass -= .5f;
        player.rb.gravityScale = 3;
        
        player.rb.constraints = UnityEngine.RigidbodyConstraints2D.None;
        player.UpdateSprite(player.data.FatRano); 
        player.rb.sharedMaterial = player.data.bouncyMat;
        player.GetCollider().sharedMaterial = player.data.bouncyMat;
        player.GetComponent<CircleCollider2D>().radius = 2f;
        


        player.AddAction(slamAction);
        player.AddState(bounceToggle);
        
        
    }
    public override string ToString()
    {
        return "Chub!";
    }
}