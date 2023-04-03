using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AutoScroller : MonoBehaviour
{
    public int speed;
    public int screenWidth; //for setting bounds
    public GameObject player;
    public float momentum; 
    CinemachineBrain brain;
    public GameObject vcamHolder;
CinemachineVirtualCamera vcam ;
    private float momentumAccumulated;
    //for configuring how fast the game can get
    public int momentumCaps;




    public Camera cam;
    public EdgeCollider2D edge;
    

    float width, height;

    public int z;
    void FindBoundaries()
    {
        width = 1 /(cam.WorldToViewportPoint( cam.ViewportToWorldPoint(new Vector2(0,0), Camera.MonoOrStereoscopicEye.Mono) - Vector3.left).x);//width of screen
        height = 1/(cam.WorldToViewportPoint( cam.ViewportToWorldPoint(new Vector2(0,0), Camera.MonoOrStereoscopicEye.Mono) - Vector3.down).y);//width of screen

    }
    void Awake()
    {
      
    }
    void Start()
    {
     edge = GetComponent<EdgeCollider2D>();

vcam = vcamHolder.GetComponent<CinemachineVirtualCamera>();
    
// brain = (cam == null) ? null : cam.GetComponent<CinemachineBrain>();
// vcam = (brain == null) ? null : brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        
    }
    void Setbounds()
    {
        Vector2 pointA = new Vector2(width/2, height/2);//used twice
        Vector2 pointB = new Vector2(width/2, -height/2);
        Vector2 pointC = new Vector2(-width/2, -height/2);
        Vector2 pointD = new Vector2(-width/2, height/2);

        Vector2[]  tempArray = new Vector2[]{pointA, pointB,pointC, pointD, pointA};
        edge.points = tempArray;
    }
    // Update is called once per frame
    void Update()
    {
        //a scalar to determine how much zoom we need
            float zoomDemand =  player.transform.position.y;
        var zoomVariable = Mathf.Clamp((zoomDemand/2), 0, 25);

        vcam.m_Lens.OrthographicSize = 25 + zoomVariable;
        momentumAccumulated += momentum*Time.deltaTime; 
        FindBoundaries();
        transform.position += new Vector3(speed + momentumAccumulated, 0, 0) * Time.deltaTime;
        transform.position += new Vector3(0, player.GetComponent<Rigidbody2D>().position.y- transform.position.y, 0) *10* Time.deltaTime;
        



        Setbounds();
       

    }
}
