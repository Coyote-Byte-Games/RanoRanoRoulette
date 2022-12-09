using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelScript : MonoBehaviour
{
    Image img;
    [SerializeField]
    // public Modifier[] mods;
    // Start is called before the first frame update
    void Start()
    {
        #region mockup
            
        
        int numOfMods = 5;
        for (int i = 1; i < numOfMods; i++)
        {
            //starts at 0
            //:gonna use some trig here to fill in the circle
            float x = Mathf.Cos(2*i*Mathf.PI/numOfMods); //acos didnt work, as, while it did create a binding between a linear to a more interesting function, the limits stopped it from having the same sine-like behaviour of an actual wave. The answer, in this case, was a true wave. While the geometric defenition states that sine and cosine both return angles, the algebraic defenition says that a radian input (times pi?) can give both x and y coordinates, binding a linear function to a circle or a wave that changes direction periodically. The important part is that it can give more than just an angle, and has potential geometric applications beyond the very geometric defenition. 
            float y = Mathf.Sin(2*i*Mathf.PI/numOfMods);
            Tuple<float, float> coords = new Tuple<float, float>(x,y);

            
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
