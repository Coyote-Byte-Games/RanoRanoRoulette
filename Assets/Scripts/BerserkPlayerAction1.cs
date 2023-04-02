using System.Collections;
using Unity;
using Unity.VisualScripting;
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

        // sword.transform.rotation = (Quaternion.AngleAxis(angle - 20, Vector3.forward));
        //problem child


        var inbet = ((mod.player.transform.InverseTransformVector(camDir)));

        //1/I WOULD try to do something clever with tangent here, but its best we keep to using vectors
        //2/ We get the error measured of the player's angle, then apply that error. IE + ((1, 0) - (.7, .3)) 
        //!Gross!
        Vector3 rotationCompensationVect =
            new Vector3
                (
                1 + Mathf.Cos(mod.player.rb.rotation * Mathf.Deg2Rad),
                0 + Mathf.Sin(mod.player.rb.rotation * Mathf.Deg2Rad),
                0
                );
        //angle sum theorm go brrr
        //maybe overkill but meh
        float cursorRadians = Mathf.Atan2(inbet.y, inbet.x);


        var firstAngle = cursorRadians;
        var secondAngle = mod.player.rb.rotation * Mathf.Deg2Rad;
        var finalCosine = Mathf.Cos(firstAngle) * Mathf.Cos(secondAngle) - (Mathf.Sin(firstAngle) * Mathf.Sin(secondAngle));
        var finalSine = Mathf.Sin(firstAngle) * Mathf.Cos(secondAngle) + Mathf.Sin(secondAngle) * Mathf.Cos(firstAngle);
        var finalAngle = new Vector2(finalCosine, finalSine).normalized;

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
        var end2 = end.normalized;
        var thing1 = Mathf.Atan2(end2.y, end2.x) * Mathf.Rad2Deg;




        for (int i = 0; i < 5; i++)
        {//TODO CLEAN THIS 
            sword.transform.position += end / 5;//* (thing) 

            sword.GetComponent<BoxCollider2D>().enabled = true;
            //by the end of the 5 seconds, have the thing rotated 90 degrees

            Debug.Log(thing1 + " " + sword.GetComponent<Rigidbody2D>().rotation);
            yield return new WaitForSeconds(.0125f);
        }
        sword.GetComponent<Rigidbody2D>().SetRotation(thing1);
        sword.transform.rotation = Quaternion.Euler(end2.x, end2.y, 0);

        yield break;

    }

    public Sprite GetIcon()

    {
        return mod.GetIcon();
    }
}