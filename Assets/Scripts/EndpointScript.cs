using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndpointScript : MonoBehaviour
{
    // Start is called before the first frame update
    private bool triggered = false;
    public SFXManagerSO sfxMan;
    private GameManagerScript gm;
    public void Start()
    {
        gm =  FindAnyObjectByType<GameManagerScript>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInChildren<RanoScript>() is not null && !triggered)
        {
            triggered = true;
            gm.audioSource.PlayOneShot(sfxMan.GetTrack(SFXManagerSO.Music.VictoryJingle), 0.5f);

            // MenuScript.instance.LoadSceneSoSoSoftly(SceneEnum.GAMEOVER);
           gm.LoadNextLevel();        
            }
    }
}
