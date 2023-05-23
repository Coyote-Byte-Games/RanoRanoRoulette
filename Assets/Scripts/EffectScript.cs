using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    public float lifetimeSeconds;
    private float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetimeSeconds);
    }

    // Update is called once per frame
    // void Update()
    // {
    //     timeElapsed += Time.deltaTime;

    //     if (timeElapsed >= LifetimeSeconds)
    //     {
    //         Destroy();
    //     }
    }
