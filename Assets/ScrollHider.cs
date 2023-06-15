using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollHider : MonoBehaviour
{
    public GameObject scrollKnob;
    public RectTransform trans;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //manual hiding of the scrolls
        Vector3[] corners = new Vector3[4];
        trans.GetWorldCorners(corners);
        scrollKnob.SetActive(!(corners[0].y - transform.position.y < -100) );
        
        
    }
}
