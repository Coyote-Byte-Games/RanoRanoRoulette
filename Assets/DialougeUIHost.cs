using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialougeUIHost : MonoBehaviour
{
    private List<UnityEvent> events;
    public GameObject dialougeMasterObject;
    public TMPro.TextMeshProUGUI wordsGUI;
    public TMPro.TextMeshProUGUI rCharacterName;
    public GameObject Lsprite;
    public GameObject Rsprite;
    private DialougeContainer[] dialouges;

    private readonly List<char> punctuation = new List<char>(){'.', '!', ',', '?'};

    private Queue<DialougeNode> dialougeQueue;


    // Start is called before the first frame update
    void Start()
    {
        dialougeQueue = new Queue<DialougeNode>();

         this.events = new List<UnityEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public void DisplayNextSentence()
    {
        if (dialougeQueue.Count == 0)
        {
            EndConvo();
            return;
        }
        DialougeNode iteration = dialougeQueue.Dequeue();
        


        //in case the player skips ahead
        StopAllCoroutines();
        StartCoroutine(TypeOutSentence(iteration.sentence));

        Rsprite.GetComponent<Image>().sprite = iteration.RSprite;
        Lsprite.GetComponent<Image>().sprite = iteration.LSprite;
      
    }
    public void BeginDialouge(DialougeContainer[] dialougeContainers, int index = 0)
    {
        //This is just so we have a public interfaces
        dialouges = dialougeContainers;
        StartDialougeContainer(dialouges[index]);
    } 
     private void StartDialougeContainer(DialougeContainer dialougeDataHolder)
    {

        dialougeMasterObject.gameObject.SetActive(true);
        //add all of the Dialouge level items
        rCharacterName.text = dialougeDataHolder.name;
        dialougeQueue.Clear();
        this.events = new List<UnityEvent>(dialougeDataHolder.actions);
        //add all of the incoming dialouge
        //we're just gonna do the crap implement first
        //todo: make this not shit
        foreach (var item in dialougeDataHolder.dialouges)
        {
            dialougeQueue.Enqueue(item);
        }Debug.Log(dialougeQueue.Count + " fdsasfd");
        //begin converstation with new dialouge in
        DisplayNextSentence();
    }
    private IEnumerator TypeOutSentence(string input)
    {

        wordsGUI.text = string.Empty;
        var sentence = input;
        foreach (char item in input.ToCharArray())
        {
                float waitTime = 0.01f;
                wordsGUI.text += item;
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
        // dialougeMasterObject.gameObject.SetActive(false);
        dialougeMasterObject.gameObject.SetActive(false);

        foreach (var item in events)
        {
            item?.Invoke();
        }
     
        

    }
        
}
