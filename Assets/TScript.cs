using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayAnim());
    }


    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator PlayAnim()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(.25f);
            GetComponent<Animator>().Play("Open");
            yield break;
        }

    }
}
