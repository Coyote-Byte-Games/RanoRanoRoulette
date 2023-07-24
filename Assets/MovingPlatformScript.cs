using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 multipliedDirection;
    private float scale;
    private float repeatRate;
    private float slowFactor;
    // private GameObject prior
    public int leftBound, rightBound;
    public int leftBoundY, rightBoundY;
    public int speed;
    private int directionMult;

    void Start()
    {
        // InvokeRepeating(nameof(UpdatePosition), repeatRate: repeatRate, time: 0 );
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();

    }

    private void UpdatePosition()
    {

        transform.localPosition = new Vector3(Mathf.PingPong(Time.time * 2 * speed, rightBound - leftBound) + leftBound, Mathf.PingPong(Time.time * 2 * speed, rightBoundY - leftBoundY) + leftBoundY, transform.localPosition.z);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        var rano = col.gameObject.GetComponent<RanoScript>();
        if (rano is not null)
        {
            //We're gonna get the current direction of our platform and then add that to the Rano regularly.
            // rano.transform.position += new Vector3(Mathf.PingPong(Time.deltaTime * 2 * speed, rightBound - leftBound) + leftBound, Mathf.PingPong(Time.time * 2 * speed, rightBoundY - leftBoundY) + leftBoundY, transform.localPosition.z);
            // rano.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            rano.transform.SetParent(transform);
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
         var rano = col.gameObject.GetComponent<RanoScript>();
        if (rano is not null)
        {
            //We're gonna get the current direction of our platform and then add that to the Rano regularly.
            // rano.transform.position += new Vector3(Mathf.PingPong(Time.deltaTime * 2 * speed, rightBound - leftBound) + leftBound, Mathf.PingPong(Time.time * 2 * speed, rightBoundY - leftBoundY) + leftBoundY, transform.localPosition.z);
            rano.transform.SetParent(null);
        }
    }
}

// public class MyScriptGizmoDrawer
// {
//     [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
//     static void DrawGizmoForMyScript(MovingPlatformScript scr, GizmoType gizmoType)
//     {
//         Vector3 position = scr.transform.position;
//         Gizmos.DrawLine(position + scr.leftBound * Vector3.right, position + scr.leftBound * Vector3.right);
//     }
// }