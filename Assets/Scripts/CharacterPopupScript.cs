using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPopupScript : MonoBehaviour
{
    public void SetImageExtra(GameObject arg)
    {
        if (arg)
        {
        arg.transform.SetParent(transform.GetChild(0));
        arg.transform.localPosition= new Vector3(10, -3, 0);     
        }
       
    }
    public void SetText(string input)
    {
          GetComponentInChildren<TextMesh>().text = input;
    }
    // Start is called before the first frame update
    
    
    void Start()
    {
      
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
