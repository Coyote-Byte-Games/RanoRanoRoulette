using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WheelScript : MonoBehaviour
{
    public List<Modifier> mods = new List<Modifier>();
    public float torque;
    public float velocity;
    public GameObject spinner;
public Camera cam;

public Rigidbody2D rb;
public int scale;
//the size of the wheel
[SerializeField]
public int UILayer = 5;

private int numOfMods;
    public Material[] mats;
     Mesh[] meshes ;

    List<GameObject> meshers = new List<GameObject>();
    public float _xoffset;
    public float _yoffset;

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
    void Update()
    {
     
        spinner.GetComponent<Rigidbody2D>().position.Set(transform.position.x + _xoffset , transform.position.y + _yoffset);
    }
    void Start()
    {
       
    }
    void Awake()
    {
        //? Perhaps do init. and dependencies in Awake, and actions in start?
Debug.Log("wake up wake up");
        rb = GetComponent<Rigidbody2D>();
        AddMods();
        numOfMods = mods.ToArray().Length;
        
       try
       {
          //store all of our coords in here
         Tuple<float, float>[] coordArr = new Tuple<float, float>[numOfMods];
        //each mesh represents a modifier on the wheel
        meshes = new Mesh[numOfMods];
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
            meshers.Add(mesher);
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
        
    

    // Update is called once per frame
   
   
//+= just sets ig
rb.position = new Vector3(0, 70, transform.position.z);
       }
       catch (System.Exception)
       {
        
        throw;
       }
      
    }

    private void AddMods()
    {
       Debug.Log("call me babee");
       mods = ModifierLibrary.GenerateRandomMods(4);
    }

    GameObject CreateMeshObject()
   {
    GameObject mesh = new GameObject();
    mesh.AddComponent<MeshRenderer>();
    mesh.AddComponent<MeshFilter>();
    mesh.GetComponent<MeshFilter>().mesh = new Mesh();
    mesh.transform.position.Set(0, 0, transform.position.z);
    return mesh;
   }
    public Modifier Launch()
    {
        
        //! IEnum, out Mod, with yield return didn't work; see Error CS1623
        //! Another is that async/await probably shouldn't be mixed with coroutines

        //?will this disrupt things, as the mod is returned?
        //needs to call/manage the wheel showing on screen, then dispersing a mod
       int index = UnityEngine.Random.Range(0, numOfMods-1);//exclusive 2nd?
       Debug.Log(index);
       Modifier selectedMod = mods[index];
       //!THE MODS ARE NULL AT THIS POINT 
       
     

      
       StartCoroutine(MoveWheel(new Vector3(rb.position.x, 30, transform.position.z), velocity));
       //i am going to cry
       //exectuion will not continue untill the angular velocity has chillded

       //haha 
       //it doesnt work
       //!UnityException: get_angularVelocity can only be called from the main thread.


      //some way to find when the wheel has slowed


      //grab a random mod, then remove it from the list 
       Debug.Log($"Receiving {selectedMod}!");
       
       mods.Remove(selectedMod);
       //! This may have disasterous consequences.
       
       return selectedMod;
    
      


    }


      public IEnumerator MoveWheel(Vector3 destination, float velocity)
    {
       
        //transform.localPosition = Vector3.MoveTowards( transform.position, destination, 55);
        //todo fix the deltatime
        //todo add random torque
        rb.AddForce(new Vector2(0, 1*velocity));
        rb.AddTorque(torque * Time.deltaTime);
        spinner.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1*velocity));
       
       for (;;)
       {
      
        if (rb.position.y <= destination.y)
        {
            spinner.GetComponent<Rigidbody2D>().constraints =RigidbodyConstraints2D.FreezePosition;
              rb.constraints =RigidbodyConstraints2D.FreezePosition;
            //stops execution
              yield break;
            
        }

        yield return null;
       }
       //gonna blow my head off
       
           
     
    }
}

//TODO
//// get wheel gen
//// get final wheel slice done
// colors
//child
// refine
//