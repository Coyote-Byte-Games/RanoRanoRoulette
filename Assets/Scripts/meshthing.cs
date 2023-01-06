using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class meshthing : MonoBehaviour
{
     Mesh mesh;
    Vector3[] verts;
    int[] triangles;
    // Start is called before the first frame update
    void Start()
    {
        //create our own mesh in code, but use filter
        mesh = new Mesh();
        // DO NOT ever use the renderer, just the mesh itself and the filter
        GetComponent<MeshFilter>().mesh = mesh;
        verts = new Vector3[]
        {
            new Vector3(0, 0 , 0),
            new Vector3(0, 1 , 0),
            new Vector3(1, 0 , 0)
        };

        triangles = new int[]
        {
            0,1,2
        };


        mesh.Clear();
        //we interact with the mesh, but not the renderer  f
     mesh.vertices = verts;
        mesh.triangles = triangles;
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
