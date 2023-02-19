using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdoptionModifier : UnityEngine.Object, IModifier, ICompanionModifier
{
    public bettertestplayablescript player;
    public GameObject dogPrefab;
    public IEnumerator ContinuousEffect(bettertestplayablescript bettertestplayablescript)
    {
      yield break;
    }
public AdoptionModifier()
{
    
}
    public void OnStartEffect(bettertestplayablescript player)
    {
        this.dogPrefab = player.data.AdoptionDog;
        var dogInstance = Instantiate(dogPrefab, player.transform.position + Vector3.up*10, Quaternion.identity);
        dogInstance.GetComponent<AdoptionDogScript>().SetTarget(player);
    }

    public void SetPlayer(bettertestplayablescript player)
    {
      this.player = player;
    }

    public void SetPlayerEffects(bettertestplayablescript player)
    {
        return;
    }
    
    public override string ToString()
    {
        return "Adoption!";
    }

    public Sprite GetIcon()
    {
        return player.data.AdoptionDog.GetComponent<SpriteRenderer>().sprite;
    }
}
