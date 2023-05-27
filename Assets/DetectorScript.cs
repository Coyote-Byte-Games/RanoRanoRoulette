using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectorScript : MonoBehaviour
{
    //the tie to the parents reaction to something coming into contact with it
    public UnityEvent parentReaction;
    //when the player leaves the zone
    public UnityEvent parentNegativeReaction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<RanoScript>() is not null)
        {
            parentReaction?.Invoke();
        }
         
    }
     public void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<RanoScript>() is not null)
        {
                  parentNegativeReaction?.Invoke();

        }
    }
}
