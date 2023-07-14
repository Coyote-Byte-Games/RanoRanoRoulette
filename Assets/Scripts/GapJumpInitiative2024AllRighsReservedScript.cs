using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapJumpInitiative2024AllRighsReservedScript : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Detection")]
    public float radius = 1;
    public float detectionInverseSensitivity = .2f;
    [Header("Editor")]
    public bool drawGizmos = false;
    
    public bool ShouldJump()
    {
        
        bool rTrigger = Physics2D.OverlapCircle(transform.position + Vector3.down +  Vector3.right * radius, detectionInverseSensitivity);
        bool lTrigger = Physics2D.OverlapCircle(transform.position + Vector3.down +  Vector3.left  * radius, detectionInverseSensitivity);
        return lTrigger || rTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        if (drawGizmos)
        {
           Debug.DrawRay(transform.position - new Vector3( radius, 1, 0), 2 * Vector3.right * radius, Color.red); 
           
        }
        
    }
}
