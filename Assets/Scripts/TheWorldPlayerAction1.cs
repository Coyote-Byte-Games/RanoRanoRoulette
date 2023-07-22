using System;
using System.Collections;
using System.Linq;
using Unity;
using Unity.VisualScripting;
using UnityEngine;

public class TheWorldPlayerAction1 : UnityEngine.Object, IPlayerAction
{
    public static bool TheWorldActive;
    private bool running;
    private float CD;
    public float dutyTime = 5;
    public TheWorldModifier mod;
    public Sprite GetIcon()
    {
        return mod.GetIcon();
    }

    private IEnumerator Active()
    {
        running = true;
        //This is using the AS of Rano's entity base to play Rano's entity base soundManager
        mod.player.GetComponent<EntityBaseScript>().AS.PlayOneShot(mod.player.GetComponent<EntityBaseScript>().soundManager.GetClip(SFXManagerSO.Sound.ZAWARDO), .5f);
        yield return new WaitForSeconds(1.75f);
        TheWorldActive = true;

        //todo rework this to events
        FreezeBehaviour[] characters = FindObjectsOfType<FreezeBehaviour>();

        #region freezes anything that isn't rano


        foreach (var item in characters)
        {
            if (item.gameObject.GetComponent<RanoScript>())
            {
                continue;
            }
            else
            {
                foreach (var fMB in item.GetComponents<FreezeBehaviour>())
                {
                    fMB.Freeze();
                }
            }
        }
        #endregion
        //darkens the background
        ChangeBG(-1);
        float offset = 1.5f;
        yield return new WaitForSeconds(dutyTime - offset);
        //begins unfreezing
              mod.player.GetComponent<EntityBaseScript>().AS.PlayOneShot(mod.player.GetComponent<EntityBaseScript>().soundManager.GetClip(SFXManagerSO.Sound.Timemovesagain), .75f);

        #region Unfreezes


        yield return new WaitForSeconds(offset);
        TheWorldActive = false;
        foreach (var item in characters)
        {
            try
            {
                if (item.gameObject.GetComponent<RanoScript>())
                {
                    continue;
                }
                else
                {
                    item.UnFreeze();

                }
            }
            catch (System.Exception)
            {


            }


        }
        #endregion
        ChangeBG(1);
        running = false;
        yield break;

    }

    ///<summary>
    ///Changes the shade of the background in the level
    ///</summary>
    private void ChangeBG(int polarity)
    {
        var goArray = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject item in goArray)
        {
            //bg layer
            if (item.layer == 8)
            {
                try
                {
                    var sr = item.GetComponent<SpriteRenderer>();
                    sr.color = sr.color + polarity * new Color(.5f, .5f, .5f, 0) * 0.5f;
                }
                catch (System.Exception)
                {


                }

            }
        }
    }

    public void Run()
    {
        if (!running)
        {
            mod.player.StartCoroutine(Active());
            CD = 20;
        }

    }

    public bool OnCoolDown()
    {
        return CD > 0;
    }

    public void DecrementCD()
    {
        this.CD -= Time.deltaTime;
    }

    public TheWorldPlayerAction1(TheWorldModifier mod)
    {
        this.mod = mod;
    }
}