using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndpointScript : MonoBehaviour
{
    // Start is called before the first frame update
  

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInChildren<RanoScript>() is not null)
        {

            // MenuScript.instance.LoadSceneSoSoSoftly(SceneEnum.GAMEOVER);
            FindAnyObjectByType<GameManagerScript>().GameOver();        
            }
    }
}
