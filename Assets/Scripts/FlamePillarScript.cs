using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePillarScript : FreezableMonoBehaviour
{
    
    public int length;
    public float rotationSpeed;

    private GameObject[] segments;
    public GameObject child;
    // public Sprite texture;
    public GameObject segmentPrefab;
    private Vector2 beam;
    private Rigidbody2D rb;
    public void Start()
    {
         rb =GetComponent<Rigidbody2D>(); 
    }
    public void FixedUpdate()
    {
        
        if (!frozen)
        {
        rb.SetRotation(rb.rotation + rotationSpeed);
        }
    }

}
