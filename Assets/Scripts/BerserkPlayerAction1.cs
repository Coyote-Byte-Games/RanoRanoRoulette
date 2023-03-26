using System.Collections;
using Unity;
using UnityEngine;

public class BerserkPlayerAction1 : IPlayerAction
{
    GameObject sword;
    private BerserkModifier mod;
    public BerserkPlayerAction1(BerserkModifier modSource)
    {
        this.mod = modSource;
    }
    void IPlayerAction.Run()
    {
        if (!mod.player.Grounded())
        {
            return;
        }
        sword = ((GameObject)mod.sword);
        //When the action is run, in this case, flinging the player forward.

        //bind to the sword
        Vector2 camDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mod.player.transform.position).normalized;
        float angle = Mathf.Atan2(camDir.y, camDir.x) * Mathf.Rad2Deg - mod.player.transform.rotation.eulerAngles.z
        //this is where we take in the players rotation and act on that

        ;

        //we need to change the actual position of the sword to reflect the hitbox, and change the rotation of the model to make it look good

        sword.transform.rotation = (Quaternion.AngleAxis(angle - 20, Vector3.forward));
        var inbet = (mod.player.transform.InverseTransformVector(camDir).normalized);
        //unpacking for .Set()
        int consta = 10;//:change this for the good changes?
                        //todo charge up i think

        mod.player.StartCoroutine(MoveSwordInDir(inbet * consta));

        // sword.transform.localPosition = inbet * consta;
        sword.transform.GetChild(0).GetComponentInChildren<Animator>().SetTrigger("Swing");
        Debug.Log(camDir);




        mod.player.rb.AddForce(5000 * camDir);
    }

    private IEnumerator MoveSwordInDir(Vector3 end)
    {
        
        sword.transform.localPosition = Vector3.zero;
        sword.transform.rotation = Quaternion.Euler(0, 0, -180);
        Vector2 secondVector =  (end.x > 0 ? new Vector2(180, 180) : new Vector2(0,0));

        Vector3 mainVector = new Vector3(0, 0, (float)(
                            ((Mathf.Atan(end.y / end.x) + Mathf.PI * .5) 
                            * Mathf.Rad2Deg)));
                            
        Quaternion quat = Quaternion.Euler(mainVector + (Vector3)secondVector );//- Vector3.forward*mod.player.rb.rotation

                            
                            
        //wrong way when rightward

        for (int i = 0; i < 5; i++)
        {//TODO CLEAN THIS 
            // var thing = mod.player.rb.transform
            sword.transform.position += end / 5;//* (thing) 
            sword.GetComponent<BoxCollider2D>().enabled = true;
            //by the end of the 5 seconds, have the thing rotated 90 degrees

            sword.transform.rotation = (quat);

            yield return new WaitForSeconds(.0125f);
        }
        Debug.Log($"The datas you wanted bossman: Rotation { mod.player.rb.rotation}, and the resulting base {(mainVector).z}. The actual sword rotation: ");

        yield break;




    }

    public Sprite GetIcon()

    {return mod.GetIcon();
    }
}