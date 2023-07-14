using System.Collections;
using System.Linq;
using Unity;
using Unity.VisualScripting;
using UnityEngine;

public class TheWorldPlayerAction1 : UnityEngine.Object, IPlayerAction
{
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
        mod.player.soundManager.PlayClip(SFXManagerSO.Sound.ZAWARDO);
        yield return new WaitForSeconds(1.75f);
        FreezableMonoBehaviour[] characters = FindObjectsOfType<FreezableMonoBehaviour>();
        foreach (var item in characters)
        {
            if (item.gameObject.GetComponent<RanoScript>())
            {
                continue;
            }
            else
            {
                foreach (var fMB in item.GetComponents<FreezableMonoBehaviour>())
                {
                fMB.Freeze();
                }
            }
        }
        var goArray = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject item in goArray)
        {
            //bg layer
            if (item.layer == 8)
            {
                try
                {
                    var sr = item.GetComponent<SpriteRenderer>();
                    sr.color = sr.color - new Color(.5f, .5f, .5f, 0) * 0.5f;
                }
                catch (System.Exception)
                {


                }

            }
        }
        float offset = 1.5f;
        yield return new WaitForSeconds(dutyTime - offset);
        mod.player.soundManager.PlayClip(SFXManagerSO.Sound.Timemovesagain);

        yield return new WaitForSeconds(offset);

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
        //lazy af
        goArray = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject item in goArray)
        {
            //bg layer
            if (item.layer == 8)
            {
                try
                {
                    var sr = item.GetComponent<SpriteRenderer>();
                    sr.color = sr.color + new Color(.5f, .5f, .5f, 0) * 0.5f;
                }
                catch (System.Exception)
                {


                }

            }
        }
        running = false;

        yield break;

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