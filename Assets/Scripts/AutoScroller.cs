using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AutoScroller :FreezableMonoBehaviour
{
    public GameConfig config;
    private int ownSpeed;
    public int speed;
    public int ease;
    public int mainZoom;
    public int screenWidth; //for setting bounds
    public GameObject player;
    public float momentum;
    CinemachineBrain brain;
    public GameObject vcamHolder;
    CinemachineVirtualCamera vcam;
    private float momentumAccumulated;
    //for configuring how fast the game can get
    public int momentumCaps;



    public Camera cam;
    public EdgeCollider2D edge;


    float width, height;

    public int z;
        
    [Header ("Fun settings")]
    public bool moves;

    public bool IsObjectBehind(GameObject other)
    {
        return other.transform.position.x > -width / 2;
    }
    void FindBoundaries()
    {
        width = 1 / (cam.WorldToViewportPoint(cam.ViewportToWorldPoint(new Vector2(0, 0), Camera.MonoOrStereoscopicEye.Mono) - Vector3.left).x);//width of screen
        height = 1 / (cam.WorldToViewportPoint(cam.ViewportToWorldPoint(new Vector2(0, 0), Camera.MonoOrStereoscopicEye.Mono) - Vector3.down).y);//width of screen

    }
    void Awake()
    {

    }
    public override void Freeze()
    {
        ownSpeed = speed;
        this.speed = 0;

    }
     public override void UnFreeze()
    {
        this.speed = ownSpeed;
        
    }
   
    void Start()
    {
        mainZoom = config.zoom == 0 ? mainZoom : config.zoom;
        speed = config.scrollSpeed == 0 ? speed : config.scrollSpeed;
        edge = GetComponent<EdgeCollider2D>();

        vcam = vcamHolder.GetComponent<CinemachineVirtualCamera>();

        // brain = (cam == null) ? null : cam.GetComponent<CinemachineBrain>();
        // vcam = (brain == null) ? null : brain.ActiveVirtualCamera as CinemachineVirtualCamera;

    }
    void Setbounds()
    {
        Vector2 pointA = new Vector2(width / 2, height / 2);//used twice
        Vector2 pointB = new Vector2(width / 2, -height / 2);
        Vector2 pointC = new Vector2(-width / 2, -height / 2);
        Vector2 pointD = new Vector2(-width / 2, height / 2);

        Vector2[] tempArray = new Vector2[] { pointA, pointB, pointC, pointD, pointA };
        edge.points = tempArray;
    }
    // Update is called once per frame
    void Update()
    {


        //a scalar to determine how much zoom we need
        float zoomDemand = player.transform.position.y;
        var zoomVariable = Mathf.Clamp((zoomDemand / 2), 0, 25);
        if (!frozen && moves)
        {
            var distanceScaler = Mathf.Clamp(Vector2.Distance(transform.position + 50 * Vector3.left, player.transform.position) / ease, .5f, 1);
            transform.position += new Vector3((speed + momentumAccumulated) * distanceScaler, 0, 0) * Time.deltaTime;
            transform.position += new Vector3(0, player.GetComponent<Rigidbody2D>().position.y - transform.position.y, 0) * 10 * Time.deltaTime;

        }
        vcam.m_Lens.OrthographicSize = mainZoom + zoomVariable;
        momentumAccumulated += momentum * Time.deltaTime;
        FindBoundaries();




        Setbounds();

        //also kill any ranos that fall behind
        if ((player.transform.position.x + 50) < this.transform.position.x)
        {
         player.GetComponent<RanoScript>().Respawn(player.transform.position + new Vector3(100, 100, 0));
        }

    }
}
