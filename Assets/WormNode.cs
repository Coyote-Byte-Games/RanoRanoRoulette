using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormNode : EnemyTraitScript
{
    //Because it was a bitch to get nodes to follow the worm, our best option was to follow as independent
    private GameObject pseudoParent;
    private int _id;
    // Start is called before the first frame update
    // internal override void die()
    // {
    //     pseudoParent.GetComponent<WormAI>().killTheChildrenFromIndex(_id);
    //     base.die();
    // }
    public void SetID(int arg)
    {
       _id = arg;
    }

    // public void SetPesudoParent(GameObject arg)
    // {
    //    pseudoParent = arg;
    // }

    // public void TimeToDieKidWrapper(int timeLeft)
    // {
    //     StartCoroutine(TimeToDieKidInner(timeLeft));
    // }

    // private IEnumerator TimeToDieKidInner(int timeLeft)
    // {
    //     for(;;)
    //     {
    //         yield return new WaitForSeconds(timeLeft);
    //         die();
    //         yield break;
            
    //     }
    // }
    // Update is called once per frame


}
