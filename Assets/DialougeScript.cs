using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialougeScript : MonoBehaviour
{
    private List<char> punctuation = new List<char>(){'.', '!', ',', '?'};
    private Queue<DialougeNode> dialouges;
    // public Queue<string> sentences;
    //jesus this naming is really bad
    public GameObject dialougeMasterObject;
    public TMPro.TextMeshProUGUI name;
    public TMPro.TextMeshProUGUI dialouge;
    public Image Lsprite;
    public GameObject Rsprite;
    private List<UnityEvent> events;
    // Start is called before the first frame update
    void Start()
    {
        dialouges = new Queue<DialougeNode>();
        this.events = new List<UnityEvent>();
        Rsprite = dialougeMasterObject.transform.GetChild(1).gameObject;
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void BeginDialouge(Dialouge dialougeDataHolder)
    {
        Debug.Log(dialougeDataHolder.dialouges.Length + " is the dia count");

        dialougeMasterObject.gameObject.SetActive(true);
        //  Lsprite.sprite = dialouge.Lsprite;
        //  Rsprite = dialouge.Rsprite;
        
        //add all of the Dialouge level items
        name.text = dialougeDataHolder.name;
        dialouges.Clear();
        this.events = new List<UnityEvent>(dialougeDataHolder.actions);
       
        //add all of the incoming dialouge
        //we're just gonna do the crap implement first
        //todo: make this not shit
        foreach (var item in dialougeDataHolder.dialouges)
        {
            dialouges.Enqueue(item);
        }
        //begin converstation with new dialouge in
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        Debug.Log($"The thing is: {dialouges.Count}");
        if (dialouges.Count == 0)
        {
            EndConvo();
            return;
        }
        DialougeNode iteration = dialouges.Dequeue();
        
        Debug.Log((iteration.OwnSprite) + " here isthe sprite");


        //in case the player skips ahead
        StopAllCoroutines();
        StartCoroutine(TypeOutSentence(iteration.sentence));

        Debug.Log("The Rsprite is: " + Rsprite);
        Rsprite.GetComponent<Image>().sprite = iteration.OwnSprite;
      
    }
    private IEnumerator TypeOutSentence(string input)
    {

        dialouge.text = string.Empty;
        var sentence = input;
        foreach (char item in input.ToCharArray())
        {
                float waitTime = 0.01f;
                dialouge.text += item;
                if (punctuation.Contains(item))
                {
                    waitTime = .5f;
                }
                yield return new WaitForSeconds(waitTime);
        }
        yield break;
    }
    public void EndConvo()
    {
        Debug.Log("done here");
        // dialougeMasterObject.gameObject.SetActive(false);
        dialougeMasterObject.gameObject.SetActive(false);

        foreach (var item in events)
        {
            item?.Invoke();
        }
     
        

    }
        
}
