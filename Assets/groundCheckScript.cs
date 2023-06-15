using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheckScript : MonoBehaviour
{
    public RanoScript rano;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public void OnCollisionEnter2D(Collision2D other)
    {
        var script = other.gameObject.GetComponent<DamagingObjectScript>();
        if (script is not null) //This line might just be the closest thing to e^iPi = 1
        {
            //emulate jump when hitting
            rano.Jump();
        }
    }
}
