using System.Collections;
using Unity;
using Unity.VisualScripting;
using UnityEngine;

public class BerserkPlayerAction1 : IPlayerAction
{
    GameObject sword;
    private BerserkModifier mod;
    private Rigidbody2D rb;
    public BerserkPlayerAction1(BerserkModifier modSource)
    {
        this.mod = modSource;
    }
    void IPlayerAction.Run()
    {
        // rb = sword.GetComponent<Rigidbody2D>();

        if (!mod.player.Grounded())
        {
            return;
        }
        sword = ((GameObject)mod.sword);
      
        Vector2 camDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mod.player.transform.position).normalized;
        float angle = Mathf.Atan2(camDir.y, camDir.x) * Mathf.Rad2Deg - mod.player.transform.rotation.eulerAngles.z;
      

        var inbet = ((mod.player.transform.InverseTransformVector(camDir)));

      
       
        //angle sum theorm go brrr
        //maybe overkill but meh
        float cursorRadians = Mathf.Atan2(inbet.y, inbet.x);
#region trig stuff
       var firstAngle = cursorRadians;
        var secondAngle = mod.player.rb.rotation * Mathf.Deg2Rad;
        var finalCosine = Mathf.Cos(firstAngle) * Mathf.Cos(secondAngle) - (Mathf.Sin(firstAngle) * Mathf.Sin(secondAngle));
        var finalSine = Mathf.Sin(firstAngle) * Mathf.Cos(secondAngle) + Mathf.Sin(secondAngle) * Mathf.Cos(firstAngle);
        var finalAngle = new Vector2(finalCosine, finalSine).normalized;

#endregion

     
        int consta = 5;//:change this for the good changes?
        mod.player.StartCoroutine(MoveSwordInDir(finalAngle * consta));

        sword.transform.GetChild(0).GetComponentInChildren<Animator>().SetTrigger("Swing");
        //  (camDir);



        mod.player.rb.AddForce(5000 * camDir);
    }

    private IEnumerator MoveSwordInDir(Vector3 end)
    {

        sword.transform.localPosition = Vector3.zero;
        Vector3 mainVector = new Vector3(0, 0, (float)(
                            ((Mathf.Atan2(end.y, end.x) + Mathf.PI * .5)
                            * Mathf.Rad2Deg)));
        var endNormalized = end.normalized;
        var endInDeg = Mathf.Atan2(endNormalized.y, endNormalized.x) * Mathf.Rad2Deg;
        // sword.transform.rotation.SetFromToRotation(Vector3.forward, Vector3.forward * endInDeg);
        mod.SetRotation((int)endInDeg -90);//oh my god we might have it



        for (int i = 0; i < 5; i++)
        {
            sword.transform.position += end / 5;
          
            sword.GetComponent<BoxCollider2D>().enabled = true;
            yield return new WaitForSeconds(.0125f);
        }
       
        yield break;

    }

    public Sprite GetIcon()

    {
        return mod.GetIcon();
    }
}