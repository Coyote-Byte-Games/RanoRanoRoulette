using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endsitself : MonoBehaviour
{
    // Start is called before the first frame update
    private IEnumerator kys()
    {
        yield return new WaitForSeconds(5);
        Application.Quit();
    }
    void Start()
    {
        StartCoroutine(kys());      
    }
}
