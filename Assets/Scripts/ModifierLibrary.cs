using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ModifierLibrary
{

  public static List<Modifier> GenerateRandomMods(int numOfMods)
  {
       var output =new Modifier[numOfMods];
    for (int i = 0; i < numOfMods; i++)
    {
      int n = UnityEngine.Random.Range(0,commonMods.Length);
      output[i] = commonMods[n]; 
    }
 
 Debug.Log($"haha {output}");
    return output.ToList();

  }



#region Methods
  #region Misc
    //:does nothing
    public static string BlankDescription = "unregistered";
    public static void BlankSingle(bettertestplayablescript playah)
    {
      return;
    }
    public static IEnumerator BlankCont(bettertestplayablescript playah)
    {
      yield break;
    }
#endregion
  #region Kaboom
//: Creates an explosion at the player's coordinates, and adds a random force.
    
    public static void KaboomOnStart(bettertestplayablescript playah)
    {
    playah.rb.constraints = RigidbodyConstraints2D.None;

    playah.rb.AddForce(new Vector2(2000000, 2000000) * Time.deltaTime);
    playah.rb.AddTorque(2000000* Time.deltaTime);


    
    }
    

#endregion Kaboom
  #region Confused
    //: Inverts the player's controls
      public static void ConfusedOnStart(bettertestplayablescript playah)
      {
        playah.speed *= -1;
      }
      public static void ConfusedOnEnd(bettertestplayablescript playah)
      {
        playah.speed *= -1;
      }
#endregion
  #region SpringShoes
    public static void SpringShoesOnStart(bettertestplayablescript playah)
    {
     playah.jumpPower *= 3;
     playah.rb.constraints = RigidbodyConstraints2D.None;
     // todo add jump hook that launches the player based on rotation

    
    }
    public static IEnumerator SpringShoesCont(bettertestplayablescript playah)
    {
        yield break;
    }
  #endregion
  #region Berserk
  private static string BerserkDescription = "Go nuts! Go Crazy! Lose your MIND !";
   public static void BerserkOnStart(bettertestplayablescript playah)
    {
    playah.speed *= 3;
    playah.jumpPower *= 2;
  //todo add massacre effect


    
    }
    public static IEnumerator BerserkCont(bettertestplayablescript playah)
    {
        yield break;
    }
  #endregion
#endregion Methods
#region Modifier Objects

      public static Modifier _blank = new Modifier(BlankSingle, BlankCont, BlankSingle, BlankDescription, -1);
      public static Modifier Kaboom = new Modifier(KaboomOnStart, BlankCont, BlankSingle);
      public static Modifier Confused = new Modifier(ConfusedOnStart, BlankCont, ConfusedOnEnd);
      public static Modifier SpringShoes = new Modifier(SpringShoesOnStart, BlankCont, BlankSingle);
      public static Modifier Berserk = new Modifier(BerserkOnStart, BlankCont, BlankSingle);
      

private static Modifier[] commonMods = new Modifier[]
{
Kaboom,
Confused,
SpringShoes,
Berserk

};
    

#endregion

}
