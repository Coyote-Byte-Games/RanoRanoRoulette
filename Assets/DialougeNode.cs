
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialougeNode
{
  //determines the highlighted sprite
  public bool RSpeaking;
  public bool LSpeaking;

  public string sentence;
  
  public Sprite RSprite;
  public Sprite LSprite;

}
