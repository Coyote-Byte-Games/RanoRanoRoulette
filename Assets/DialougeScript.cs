using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialougeScript : MonoBehaviour
{
    public Queue<string> sentences;
    public GameObject dialougeMasterObject;
    public TMPro.TextMeshProUGUI name;
    public TMPro.TextMeshProUGUI dialouge;
    public Image Lsprite, Rsprite;
    private List<UnityEvent> events;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        this.events = new List<UnityEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BeginDialouge(Dialouge dialouge)
    {
        dialougeMasterObject.gameObject.SetActive(true);
        //  Lsprite.sprite = dialouge.Lsprite;
        //  Rsprite = dialouge.Rsprite;
        name.text = dialouge.name;
        //state bad
        sentences.Clear();
        this.events = new List<UnityEvent>(dialouge.actions);
       
        //add all of the incoming dialouge
        foreach (var item in dialouge.sentences)
        {
            sentences.Enqueue(item);
        }
        //begin converstation with new dialouge in
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndConvo();
            return;
        }
        string sentence = sentences.Dequeue();
        dialouge.text = sentence;
      
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
