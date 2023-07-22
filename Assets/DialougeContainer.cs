
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialougeContainer
{
    public string name;
  public DialougeNode[] dialouges;
  public UnityEvent[] actions;

}
