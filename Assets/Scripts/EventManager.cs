using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    //singleton
      private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }
    }
    #region event crap
    public event Action<int> buttonPress;
  


    public void OnButtonPress(int id)
    {
        buttonPress?.Invoke(id);
    }

    
    #endregion
   
}
