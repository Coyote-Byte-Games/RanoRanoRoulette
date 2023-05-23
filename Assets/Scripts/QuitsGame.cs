using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitsGame : MonoBehaviour
{
    // Start is called before the first frame update
    private IEnumerator dies()
    {
        yield return new WaitForSeconds(2);
        Application.Quit();
    }
    void Start()
    {
      StartCoroutine(dies());  
    }

    // Update is called once per frame
  
}
