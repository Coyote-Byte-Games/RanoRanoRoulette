using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TalkativeNPC : MonoBehaviour
{
    public Dialouge[] dialouges;

    public int pseudoDetectionRadius;

    
    
    

    // public UnityEvent<GameObject, Vector2> InstantiateUE;
    public void Start()
    
    {
        // InstantiateUE.AddListener((GameObject go, Vector2 pos) =>  Instantiate(go, pos, Quaternion.identity));
        SetDetectionRadius(pseudoDetectionRadius);
    }
  
    public void SetDetectionRadius(int rad)
    {
        transform.GetChild(0).GetComponent<BoxCollider2D>().size = new Vector2(rad,rad);

    }
    private int dialougeIndex = 0;
    // public LayerMask damagingMask;
 
    public void BeginNextDialouge()
    {
        Dialouge chosen = dialouges[dialougeIndex];
        FindObjectOfType<DialougeScript>().BeginDialouge(chosen);
        dialougeIndex++;
    }
        public void BeginDialougeAtIndex(int index)
    {
        Dialouge chosen = dialouges[index];
        FindObjectOfType<DialougeScript>().BeginDialouge(chosen);
    }
    public void Shaddup()
    {
        FindObjectOfType<DialougeScript>().EndConvo();
    }
    
    // public void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (col.GetComponent<RanoScript>() is not null)
    //     {
    //     FindObjectOfType<DialougeScript>().BeginDialouge(dialouge);
    //     }
         
    // }
    //  public void OnTriggerExit2D(Collider2D col)
    // {
    //     if (col.GetComponent<RanoScript>() is not null)
    //     {
    //     FindObjectOfType<DialougeScript>().EndConvo();
    //     }
    // }
}
