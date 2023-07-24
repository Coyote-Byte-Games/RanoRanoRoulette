using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdoptionModifier : UnityEngine.Object, IModifier, ICompanionModifier
{
    public RanoScript player;
    public GameObject dogPrefab;
    private GameObject doggoInstance = null;
    public IEnumerator ContinuousEffect(RanoScript RanoScript)
    {
      yield break;
    }
public AdoptionModifier()
{
    
}
    public void OnStartEffect(RanoScript player)
    {
        this.dogPrefab = player.data.AdoptionDog;
        doggoInstance = Instantiate(dogPrefab, player.transform.position + Vector3.up*10, Quaternion.identity);
        doggoInstance.GetComponent<AdoptionDogScript>().SetTarget(player);
    }

    public void SetPlayer(RanoScript player)
    {
      this.player = player;
    }
public void OnNewModAdded(RanoScript rano)
    {
      return;
    }
    public void SetPermenantEffects(RanoScript player)
    {
        return;
    }
    
    public override string ToString()
    {
        return "Adoption!";
    }

    public Sprite GetIcon()
    {
          return (Sprite)Resources.Load("Mod Icons\\dog");
    }

    public void OnEndEffect(RanoScript player)
    {
    

        Destroy( doggoInstance);
    }

    public void EnableContinuousEffect()
    {
               return;

    }

    public void DisableContinuousEffect()
    {
       return;
    }
}
