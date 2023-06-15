using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WantedManModifier : UnityEngine.Object, IModifier, IMovementModifier, IJumpModifier
{
    private RanoScript player;
    public GameObject scopePrefab = null;

 

  public override string ToString()
    {
        return "Wanted Man!";
    }

    public Sprite GetIcon()
    {
      return (Sprite)Resources.Load("Mod Icons\\wantedman");

    }

    

    public void OnStartEffect(RanoScript player)
    {
    scopePrefab.GetComponent<WMScopeScript>().target = player.transform;
    }

    public void SetPlayer(RanoScript player)
    {
       this.player = player;
       scopePrefab = player.data.WantedManPrefab;
       scopePrefab.GetComponent<WMScopeScript>().source = player.gameManager.audioSource;
    }

    public void SetPlayerEffects(RanoScript player)
    {
     
    }
       public IEnumerator ContinuousEffect(RanoScript RanoScript)
    {
      //Creates a scope every 15 seconds. The scope follows for about 5s, despawns, and is then spawned 10 seconds later. 1/3 duty cycle?

      for (;;)
      {
          Instantiate(scopePrefab, RanoScript.rb.position - new Vector2(50,50), Quaternion.identity);
        yield return new WaitForSeconds(15f);
      }
      
    }

    public void OnNewModAdded(RanoScript rano)
    {
      return;
    }
}