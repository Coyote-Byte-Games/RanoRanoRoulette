using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePillarScript : MonoBehaviour
{
    
    public int length;
    public float rotationSpeed;

    private GameObject[] segments;
    // public Sprite texture;
    public GameObject segmentPrefab;
    private Vector2 beam;

    
    // Start is called before the first frame update
    void Start()
    {
        segments = new GameObject[length+1];

        beam = Vector2.right * beam;
        for (int i = 0; i <= length; i++)
        {
            GameObject segment = Instantiate(segmentPrefab);
            segment.name = "pillar piece " + i;
            segments[i] = segment;
            segment.transform.SetParent(transform);
            segment.transform.position= new Vector2(5, i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var degreeDelta = rotationSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0,0, degreeDelta));
    }
}
