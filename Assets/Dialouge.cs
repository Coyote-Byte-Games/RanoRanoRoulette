
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Dialouge
{
    public string name;
  public DialougeNode[] dialouges;


  // public Sprite Lsprite;
  // public Sprite Rsprite;
  //determines the highlighted sprite
  // public bool Speaking;


  // [TextArea(3,10)]
  // public string[] sentences;
  
  // public Sprite[] OwnSprite;

  public UnityEvent[] actions;

}
