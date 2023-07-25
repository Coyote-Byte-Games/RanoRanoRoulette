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
    public void OnNewModAdded(RanoScript rano)
    {
        return;
    }

    public IEnumerator ContinuousEffect(RanoScript player)
    {
        for (; ; )
        {
            player.UpdateSprite(player.data.FatRano);

            if (player.Grounded() && player.rb.sharedMaterial == player.data.normalMat && bounceEnabled) //if the player isnt bouncy but grounded
            {
                yield return new WaitForSeconds(.2f);
                player.rb.sharedMaterial = player.data.bouncyMat;
                // player.rb.sharedMaterial.bounciness /= (player.rb.mass/2);

            }
            if (!bounceEnabled)
            {
                //lazy fix
                player.rb.sharedMaterial = player.data.normalMat;

            }
            //player.transform.Rotate(0, 0, -0.02f * player.GetVel());

            player.transform.GetChild(1).transform.Rotate(0, 0, -.14f * player.GetVel(), Space.Self);

            HandleImpactFX();







            yield return null;

        }
    }

    public Sprite GetIcon()
    {

        return (Sprite)Resources.Load("Mod Icons\\beachball");
    }

    public void OnEndEffect(RanoScript player)
    {
        player.rb.constraints = UnityEngine.RigidbodyConstraints2D.FreezeRotation;
        player.gameObject.transform.localScale.Set(1, 1, 0);
        player.rb.sharedMaterial = null;
        player.GetCollider().sharedMaterial = null;

    }


    //to invoke repeating?
    private void HandleImpactFX()
    {
        //fuck it

        // //only want this to happen if the bounce is enabled
        // if (!bounceEnabled)
        // {
        //     return;
        // }


        // var direction = Quaternion.Euler(0, 0, player.GetShitAcceleration());
        // if (Mathf.Abs(player.GetHomemadeAcceleration()) > player.slamCutoff)
        // {
        // Instantiate(slamEffect, player.transform.position, direction);

        // }

    }

    public void OnStartEffect(RanoScript player)
    {

    }

    public void SetPlayer(RanoScript player)
    {
        this.player = player;
        slamEffect.transform.lossyScale.Set(3f, 5f, 0f);


    }

    public void SetPermenantEffects(RanoScript player)
    {




        player.hatHolder.transform.localScale = new Vector2(2.5f, 2.5f);
        slamAction = new BeachBallPlayerAction1(this);
        bounceToggle = new BeachBallPlayerState1(this);
        player.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        player.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        player.SetJumpSFX(player.entityBase.soundManager.GetClip(SFXManagerSO.Sound.dodgeball));


        player.jumpRadius += 2;
        // player.rb.mass -= .5f;
        // player.rb.gravityScale = 3;

        player.rb.constraints = UnityEngine.RigidbodyConstraints2D.None;
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