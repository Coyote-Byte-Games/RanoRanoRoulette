using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitiveAirAI : MonoBehaviour
{
    bool doneScanning = false;
    [HideInInspector]
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Scan());
    }
    public bool DoneScanning()
    {
        return doneScanning;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Scan()
    {
        for (; ; )
        {
            //This loop keeps checking for the target. If it finds a terget, it ends.
            yield return new WaitForSeconds(.25f);

            try
            {
                 target = FindObjectOfType<RanoScript>().transform;
            }
            catch (System.Exception)
            {
                target = null;
            }
            if (target is not null)
            {
                break;
            }
        }
        doneScanning = true;
        yield break;
    }
}
