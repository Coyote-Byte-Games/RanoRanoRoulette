using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePillarScript : MonoBehaviour
{
    
    public int length;
    public float rotationSpeed;

    private GameObject[] segments;
    public GameObject child;
    // public Sprite texture;
    public GameObject segmentPrefab;
    private Vector2 beam;

    
    // Start is called before the first frame update
 
    // Update is called once per frame
    void Update()
    {
        // var degreeDelta = rotationSpeed * Time.deltaTime;
        // transform.Rotate(new Vector3(0,0, degreeDelta));
    }
}
