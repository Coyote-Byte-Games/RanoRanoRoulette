using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheckScript : MonoBehaviour
{
    public RanoScript rano;

     public void OnCollisionEnter2D(Collision2D other)
    {
        var script = other.gameObject.GetComponentInChildren<EnemyTraits>();
        if (other.gameObject.layer != 6)
        {
            //emulate jump when hitting
            rano.Jump(useJumpCredit: false, overrideBerserk:true, playSound: true);

             rano.animator.SetTrigger("JumpTrick");
                rano.jumpsUsed = 0;
                var entityBaseScript = script.GetComponent<EntityBaseScript>();
                if (entityBaseScript != null)
                {
                    entityBaseScript.TakeDamage(1, false);
                }
        }
    }
}
