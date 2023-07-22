using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoilScript : MonoBehaviour
{
    // for holding all the children

    //the textures to be used

    //Each Column is a frame, each row is another slice
    GameObject[] slices;
    //stupid worthless botch but I tried peace and Unity wanted violence
    private int x = 0;

    
    public ArrayLayout<Sprite> layout = new ArrayLayout<Sprite>(4, 7);
    // Start is called before the first frame update [Space(18)]
    public GameObject renderer;
    public BoxCollider2D longStick;
    //length of beam
    public int length;
    public FreezeBehaviour freezeBehaviour;
    //The point is to use the rows on repeat to create the line cont. Using the columns to animate
    void Start()
    {
        //get the number of slices demanded
        int demand = length;
        longStick.size = new Vector2(5, demand * 7);
        //Grab the corresponding sprites, apply them to corresponding gameobjects
        slices = new GameObject[demand];
        for (int i = 0; i < demand; i++)
        {

            //this is shit
            slices[i] = new GameObject($"LightningLink{i}", typeof(BoxCollider2D), typeof(SpriteRenderer));
            slices[i].GetComponent<SpriteRenderer>().sprite =
            layout.GetIndex(0, i % layout.rowCount);
            //parent the renderer to these gameobjects
            slices[i].transform.SetParent(renderer.transform);
            slices[i].layer = slices[i].transform.parent.gameObject.layer;

            slices[i].transform.localPosition = Vector2.down * ((i * (10.29f - 7.12f) + 2.91f));//lmao

        }
        //To emulate animation for the sprites
        //this makes my soul cry. Unfortunately, we cannot yield return x and yield return new WaitForSeconds(y)...
        InvokeRepeating(nameof(UpdateSprites), 0, .1f);


    }



    private void UpdateSprites()
    {
        if (freezeBehaviour.frozen)
        {
            return;
        }

        //to keep track of the current "frame"
        for (int i = 0; i < slices.Length; i++)
        {
            slices[i].GetComponent<SpriteRenderer>().sprite =
            layout.GetIndex(x % layout.colCount, i % layout.rowCount);
            x += 1;
        }


    }

    // Update is called once per frame

}
