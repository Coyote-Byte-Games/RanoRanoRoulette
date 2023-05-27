
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Dialouge
{
  public UnityEvent[] actions;
  public Sprite Lsprite;
  public Sprite Rsprite;


  [TextArea(3,10)]
  public string[] sentences;

  public string name;
}
