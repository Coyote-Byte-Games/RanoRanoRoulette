using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    private int _instanceId;
    private AudioSource AS;
    public AudioClip[] SFX;
    // Start is called before the first frame update
    void Start()
    {
        this._instanceId =transform.parent.gameObject.GetInstanceID();
        EventManager.instance.buttonPress += this.Open;
        this.AS = FindObjectOfType<AudioSource>();
    }

    // Update is called once per frame
  
   
    public void Open(int id)
    {
        if (id == _instanceId)
        {
        GetComponent<Animator>().SetTrigger("Open");
        AS.PlayOneShot(SFX[0]);
        }
    }
}
