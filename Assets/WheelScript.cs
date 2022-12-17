using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelScript : MonoBehaviour
{

public int scale;
//the size of the wheel
[SerializeField]
public int UILayer = 5;
public int numOfMods;
    public Material[] mats;
    

 void UpdateMesh(Mesh mesh, Vector3[] verts, int[] triangles)
    {
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = triangles;
    }
//all for use with the wheel rendering
//x Mesh mesh;
//x Vector3[] verts;
//x int[] triangles;

    // public Modifier[] mods;
    // Start is called before the first frame update
    void Start()
    {
        
        #region mockup


       
        //store all of our coords in here
        Tuple<float, float>[] coordArr = new Tuple<float, float>[numOfMods];
        //each mesh represents a modifier on the wheel
        Mesh[] meshes = new Mesh[numOfMods];
        //: get all of the coordinates at once
        for (int i = 0; i < numOfMods; i++)
        {
           
            //?why the 2? 
                //: to convert from deg to rad
            float x = Mathf.Cos(2*i*Mathf.PI/numOfMods); //acos didnt work, as, while it did create a binding between a linear to a more interesting function, the limits stopped it from having the same sine-like behaviour of an actual wave. The answer, in this case, was a true wave. While the geometric defenition states that sine and cosine both return angles, the algebraic defenition says that a radian input (times pi?) can give both x and y coordinates, binding a linear function to a circle or a wave that changes direction periodically. The important part is that it can give more than just an angle, and has potential geometric applications beyond the very geometric defenition. 
            float y = Mathf.Sin(2*i*Mathf.PI/numOfMods);
            coordArr[i] = new Tuple<float, float>(x,y);
        }

        //:create the meshes from the thing
        for (int i = 0; i < numOfMods; i++)
        {
            //:some config for it
            GameObject mesher = CreateMeshObject();
            mesher.transform.SetParent(transform);
            mesher.transform.position += new Vector3(0,0,100);
            mesher.gameObject.layer = UILayer;
            //starts at 0
            //:using meshes for wheel
           
            meshes[i] = mesher.GetComponent<MeshFilter>().mesh;
            
            float x = coordArr[i].Item1;
            float y = coordArr[i].Item2;

            //world's worst circular array
            int lastIndex = i -1;
            if(lastIndex == -1)
            {
                lastIndex = numOfMods-1;
            }
              Vector3[] verts = new Vector3[]
            {
                //want coords at 
                /*origin
                * last slice's coords
                * Current slices coordinates
                */

               

                //!IVE GOTCHA WHERE I WANT YOU
                new Vector3(0,0,0)*scale,
                new Vector3(x,y,0)*scale,
                new Vector3(coordArr[lastIndex].Item1,coordArr[lastIndex].Item2,0)*scale
};
            int[] triangles = new int[]
            {
                0,1,2
            };

            //:meshfilters own meshes, not the other way
            
            UpdateMesh(mesher.GetComponent<MeshFilter>().mesh, verts, triangles);
            mesher.GetComponent<MeshRenderer>().material = mats[i];
           

        }
        #endregion
    

    // Update is called once per frame
   GameObject CreateMeshObject()
   {
    GameObject mesh = new GameObject();
    mesh.AddComponent<MeshRenderer>();
    mesh.AddComponent<MeshFilter>();
    mesh.GetComponent<MeshFilter>().mesh = new Mesh();
   
    return mesh;
   }
   

}
}

//TODO
//// get wheel gen
//// get final wheel slice done
// colors
//child
// refine
//