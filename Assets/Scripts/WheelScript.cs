using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WheelScript : MonoBehaviour
{


    public GameObject textPopup;
    public Rigidbody2D rb;
    public ModifierManager modMan;
    public float radius;
    public int scale;
    //the size of the wheel
    [SerializeField]
    public int UILayer = 5;

    public GameData data;
    public Material[] mats;
    Mesh[] meshes;



    List<GameObject> meshers = new List<GameObject>();
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

    void Awake()
    {
        /**
        If there are 3, we will FIGURE IT OUT
        If there are only 2 mods, we will generate 2 squares.
        If there is 1, we will generate a large square 
        */


        rb = GetComponent<Rigidbody2D>();


        //store all of our coords in here
        //each mesh represents a modifier on the wheel
        Tuple<float, float>[] coordArr = new Tuple<float, float>[modMan.GetNumOfMods()];


        meshes = new Mesh[modMan.GetNumOfMods()];

        switch (modMan.GetNumOfMods())
        {
            case 1:
                GenerateOne();
                break;
            case 2:

                GenerateTwo();
                break;
            case 3:

                GenerateThree(coordArr);
                break;

            default:
                GenerateMoreThanThree(coordArr);
                break;
        }

        //+= just sets ig
        rb.position = new Vector3(0, 70, transform.position.z);

    }

    private void GenerateTwo()
    {
        //ok so like this contains the data for the bottomMost and bottomRight verticies
        //And we're gonna screw with it to make all of the things because haha
        //B = bottom, T = top, M = middle, R = right, L = left. In order variable names

        //float mid_offset;
        Vector2 BL = new Vector2(-1, -1).normalized * scale;
        Vector2 BM = new Vector2(0, -1).normalized * scale;
        Vector2 BR = new Vector2(1, -1).normalized * scale;
        Vector2 TL = new Vector2(-1, 1).normalized * scale;
        Vector2 TM = new Vector2(0, 1).normalized * scale;
        Vector2 TR = new Vector2(1, 1).normalized * scale;

        var meshTriangles = new int[] { 0, 1, 3, 2, 3, 1 };

        for (int i = 0; i < modMan.numOfMods; i++)
        {
            GameObject mesher = CreateMeshObject(i);
            meshers.Add(mesher);
            mesher.transform.SetParent(transform.GetChild(0));
            mesher.transform.position = Vector3.forward * -150;
            mesher.gameObject.layer = UILayer;
            //starts at 0
            //:using meshes for wheel

            meshes[i] = mesher.GetComponent<MeshFilter>().sharedMesh;

        }
        //Generate the points where we create the meshes
        //formula for diagonal of square is √2(sideLength)
        //Get corner via distance by normalized vector going in theat direction


        UpdateMesh(meshers[0].GetComponent<MeshFilter>().mesh, new List<Vector3> { BM, BR, TR, TM }.ToArray(), meshTriangles);
        meshers[0].transform.GetChild(0).transform.localPosition = new Vector3(1, 0, 0) * scale / 1.75f;
        meshers[0].transform.GetChild(0).transform.localScale = new Vector3(7.5f, 7.5f, 0);

        UpdateMesh(meshers[1].GetComponent<MeshFilter>().mesh, new List<Vector3> { BL, BM, TM, TL }.ToArray(), meshTriangles);
        meshers[1].transform.GetChild(0).transform.localPosition = new Vector3(-1, 0, 0) * scale / 2.35f;
        meshers[1].transform.GetChild(0).transform.localScale = new Vector3(7.5f, 7.5f, 0);

        //    meshes[0] = meshR;
        //    meshes[1] = meshL;
        //    meshers[0].GetComponent<MeshFilter>().mesh = meshR;
        //    meshers[1].GetComponent<MeshFilter>().mesh = meshL;
        meshers[0].GetComponent<MeshRenderer>().material = mats[0];
        meshers[1].GetComponent<MeshRenderer>().material = mats[1];


        //create the mesh objects

        //finish parenting and whatnot



    }

    private void GenerateOne()
    {
        Vector2 BL = new Vector2(-1, -1).normalized * scale;
        Vector2 BR = new Vector2(1, -1).normalized * scale;
        Vector2 TL = new Vector2(-1, 1).normalized * scale;
        Vector2 TR = new Vector2(1, 1).normalized * scale;

        var meshTriangles = new int[] { 0, 1, 2, 2, 3, 0 };

        GameObject mesher = CreateMeshObject(0);
        meshers.Add(mesher);
        mesher.transform.SetParent(transform.GetChild(0));
        mesher.transform.position = Vector3.forward * -150;
        mesher.gameObject.layer = UILayer;
        //starts at 0
        //:using meshes for wheel
        UpdateMesh(mesher.GetComponent<MeshFilter>().mesh, new List<Vector3> { BL, BR, TR, TL }.ToArray(), meshTriangles);
        mesher.GetComponent<MeshRenderer>().material = mats[0];
        mesher.transform.GetChild(0).transform.localScale = new Vector3(10f, 10, 1);

    }
    private void GenerateThree(Tuple<float, float>[] coordArr)
    {

        //the unique need here is to generate another vertex to "reach"

        //: get all of the coordinates at once
        for (int i = 0; i < modMan.GetNumOfMods(); i++)
        {

            //: to convert from deg to rad
            float x = Mathf.Cos(2 * i * Mathf.PI / modMan.GetNumOfMods()); //acos didnt work, as, while it did create a binding between a linear to a more interesting function, the limits stopped it from having the same sine-like behaviour of an actual wave. The answer, in this case, was a true wave. While the geometric defenition states that sine and cosine both return angles, the algebraic defenition says that a radian input (times pi?) can give both x and y coordinates, binding a linear function to a circle or a wave that changes direction periodically. The important part is that it can give more than just an angle, and has potential geometric applications beyond the very geometric defenition. 
            float y = Mathf.Sin(2 * i * Mathf.PI / modMan.GetNumOfMods());
            coordArr[i] = new Tuple<float, float>(x, y);
        }



        //:create the meshes from the thing
        for (int i = 0; i < modMan.GetNumOfMods(); i++)
        {

            //:some config for it
            GameObject mesher = CreateMeshObject(i);
            meshers.Add(mesher);

            // mesher.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = iconParam;

            mesher.transform.SetParent(transform.GetChild(0));
            mesher.transform.position = Vector3.forward * -150;
            mesher.gameObject.layer = UILayer;
            //starts at 0
            //:using meshes for wheel

            meshes[i] = mesher.GetComponent<MeshFilter>().mesh;

            float x = coordArr[i].Item1;
            float y = coordArr[i].Item2;

            //world's worst circular array
            int lastIndex = i - 1;
            if (lastIndex == -1)
            {
                lastIndex = modMan.GetNumOfMods() - 1;
            }
            Vector3[] verts = new Vector3[]
          {
                //want coords at 
                /*origin
                * last slice's coords
                * Current slices coordinates
                */

               

                
                //!IVE GOTCHA WHERE I WANCH YA
                new Vector3(0,0,0)*scale,
                new Vector3(x,y,0)*scale,
                new Vector3(coordArr[lastIndex].Item1,coordArr[lastIndex].Item2,0)*scale,
                new Vector3((x+coordArr[lastIndex].Item1)/2,(coordArr[lastIndex].Item2+y)/2,0 )*2*scale,


};
            int[] triangles = new int[]
            {
                0,1,2,
                 3,2,1
            };

            //:meshfilters own meshes, not the other way


            //For the icon
            mesher.transform.GetChild(0).transform.localPosition = new Vector3((coordArr[lastIndex].Item1 + x) / 2, (y + coordArr[lastIndex].Item2) / 2, 0) * scale / 1.75f;


            UpdateMesh(mesher.GetComponent<MeshFilter>().mesh, verts, triangles);

            mesher.GetComponent<MeshRenderer>().material = mats[i];


        }
    }
    private void GenerateMoreThanThree(Tuple<float, float>[] coordArr)
    {

        //: get all of the coordinates at once
        for (int i = 0; i < modMan.GetNumOfMods(); i++)
        {

            //: to convert from deg to rad
            float x = Mathf.Cos(2 * i * Mathf.PI / modMan.GetNumOfMods()); //acos didnt work, as, while it did create a binding between a linear to a more interesting function, the limits stopped it from having the same sine-like behaviour of an actual wave. The answer, in this case, was a true wave. While the geometric defenition states that sine and cosine both return angles, the algebraic defenition says that a radian input (times pi?) can give both x and y coordinates, binding a linear function to a circle or a wave that changes direction periodically. The important part is that it can give more than just an angle, and has potential geometric applications beyond the very geometric defenition. 
            float y = Mathf.Sin(2 * i * Mathf.PI / modMan.GetNumOfMods());
            coordArr[i] = new Tuple<float, float>(x, y);
        }



        //:create the meshes from the thing
        for (int i = 0; i < modMan.GetNumOfMods(); i++)
        {

            //:some config for it
            GameObject mesher = CreateMeshObject(i);
            meshers.Add(mesher);

            // mesher.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = iconParam;

            mesher.transform.SetParent(transform.GetChild(0));
            mesher.transform.position = Vector3.forward * -150;
            mesher.gameObject.layer = UILayer;
            //starts at 0
            //:using meshes for wheel

            meshes[i] = mesher.GetComponent<MeshFilter>().mesh;

            float x = coordArr[i].Item1;
            float y = coordArr[i].Item2;

            //world's worst circular array
            int lastIndex = i - 1;
            if (lastIndex == -1)
            {
                lastIndex = modMan.GetNumOfMods() - 1;
            }
            Vector3[] verts = new Vector3[]
          {
                //want coords at 
                /*origin
                * last slice's coords
                * Current slices coordinates
                */

               

                
                //!IVE GOTCHA WHERE I WANCH YA
                new Vector3(0,0,0)*scale,
                new Vector3(x,y,0)*scale,
                new Vector3(coordArr[lastIndex].Item1,coordArr[lastIndex].Item2,0)*scale
};
            int[] triangles = new int[]
            {
                0,1,2
            };

            //:meshfilters own meshes, not the other way


            mesher.transform.GetChild(0).transform.localPosition = new Vector3((coordArr[lastIndex].Item1 + x) / 2, (y + coordArr[lastIndex].Item2) / 2, 0) * scale / 1.75f;


            UpdateMesh(mesher.GetComponent<MeshFilter>().mesh, verts, triangles);

            mesher.GetComponent<MeshRenderer>().material = mats[i];


        }
    }

    GameObject CreateMeshObject(int modIconIndex)
    {
        GameObject mesh = new GameObject();
        mesh.AddComponent<MeshRenderer>();
        mesh.AddComponent<MeshFilter>();
        mesh.GetComponent<MeshFilter>().mesh = new Mesh();
        mesh.transform.position.Set(0, 0, transform.position.z);
        GameObject icon = new GameObject("icon");

        icon.transform.SetParent(mesh.transform);
        icon.gameObject.layer = UILayer;
        var sprite = modMan.mods[modIconIndex].GetIcon();
        icon.AddComponent<SpriteRenderer>().sprite = sprite;
        //for good measure
        icon.GetComponent<SpriteRenderer>().sortingOrder = 50;
        icon.transform.localScale = new Vector3(5, 5, 0);


        return mesh;
    }
    public IModifier Launch()
    {

        //?will this disrupt things, as the mod is returned?
        //needs to call/manage the wheel showing on screen, then dispersing a mod
        int index = UnityEngine.Random.Range(0, modMan.GetNumOfMods());//exclusive 2nd?
        Debug.Log("index is " + index);


        IModifier selectedMod = modMan.mods[index];




        //grab a random mod, then remove it from the list 

        var popup = Instantiate(textPopup, new Vector3(999, 0, 100), Quaternion.identity);
        popup.SetActive(true);
        popup.transform.position = new Vector3(0, 5, 20);
        var script = popup.GetComponent<PopUpScript>();
        script.SetText(selectedMod.ToString());
        Destroy(popup, 4);

        foreach (var item in modMan.mods)
        {
        }
        //! removal just gets rid of the first mod?

        modMan.mods.RemoveAt(index);

        return selectedMod;

        //why are you copying the files you dumb guy


    }

    public void FuncTest()
    {

    }

}

//TODO
//// get wheel gen
//// get final wheel slice done
// colors
//child
// refine
//