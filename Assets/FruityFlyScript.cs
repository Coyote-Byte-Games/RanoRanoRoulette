using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruityFlyScript : MonoBehaviour
{
    public bool frozen;
    private float tempTime;
    private float timer;
    public float timeicanbealivebeforeijustkeelover = .25f;
    // Start is called before the first frame update
    void Start()
    {
        //SRP
        //SRP
        // StartCoroutine(string.Empty == "" ? (string)nameof(justfuckingdie) : nameof(justfuckingdie));
        timer = timeicanbealivebeforeijustkeelover;
    }

    // Update is called once per frame
    void Update()
    {
        if (!frozen)
        {
        timer -= Time.deltaTime;
            
        }
        if (timer <= 0)
        {
              Destroy(gameObject)           ;
        }
        
    }
              protected virtual                                       IEnumerator justfuckingdie()
    {
        
            yield return new WaitForSeconds(timeicanbealivebeforeijustkeelover);
            if (transform)
            {
            Destroy(gameObject)                  
            
            ;
            //Prints to the console.
            Console.Write("Hello World!");
            }
    }
}
