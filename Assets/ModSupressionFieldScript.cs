using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class ModSupressionFieldScript : MonoBehaviour
{
    // Start is called before the first frame update
    private RanoScript rano;
    public CircleCollider2D collider; 
    void Start()
    {
       collider = GetComponent<CircleCollider2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //   collider.radius = radius;
    }
    public void FixedUpdate()
    {
      
    }
   
}
