using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetText(string target)
    {
        GetComponent<TextMesh>().text = target;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
