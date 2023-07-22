using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAI : MonoBehaviour
{
    // Start is called before the first frame update


    private bool doneScanning = false;
    private Transform[] nodes;
    [Header("Other Scripts")]
    public EntityBaseScript entityBase;
    public EnemyTraits enemyTraits;

    [Header("Body")]
    public GameObject childObjectPrefab;
    // private EnemyTraitScript traits;
    public float speed;
    public float maxNodeDistanceDelta;
    public int numberOfNodes;
    public float distancebetweenLinks;
    [Header("Pathfinding")]
    public GameObject target;

    [Header("Fun stuff")]
    public bool gravity;
    public ParticleSystem particles;
    

    ///<summary>
    ///For every single child the transform has, that node will move slightly towards the node in front of it. That is, if we were to say node[0] is the original, each node n will move towards node n-1
    ///</summary>
    private void BringFollowerNodes()
    {


        //iterate over every child. If the index is zero, we will follow the host(this gameobject). If not zero, we will follow the n-1 node.

        for (int i = 0; i < numberOfNodes; i++)
        {
            var currentnode = nodes[i];

            //If first child node, go for main object
            if (i == 0)
            {
                currentnode.position = transform.position - (Vector3)transform.GetComponent<Rigidbody2D>().velocity.normalized * distancebetweenLinks;
            }
            //If not the first child node, go for n-1
            else
            {
                currentnode.position = nodes[i-1].position - (nodes[i-1].position - currentnode.position).normalized * distancebetweenLinks;
            }
        }
    }
    public IEnumerator killTheChildrenFromIndex(int id)
   {
        for (int i = id; i < nodes.Length; i++)
        {
           
            yield return new WaitForSeconds(.1f);
            nodes[i].gameObject.GetComponent<EntityBaseScript>().die();
            
        }
        yield break;
    }
    private bool TargetInRange()
    {
        return Vector2.Distance(target.transform.position, transform.position) <= enemyTraits.aggroRange;
    }
    public void Start()
    {
        entityBase.OnDeath += DeathSubby;
        enemyTraits.EnableSearchingVFX(particles, TargetInRange);


        // traits = GetComponent<EnemyTraitScript>();
        StartCoroutine(Scan());
        nodes = new Transform[numberOfNodes];
        for (int i = 0; i < numberOfNodes; i++)
        {
            //-distancebetweenLinks * (i + 1)
            var ob = Instantiate(childObjectPrefab, new Vector3(0, 0, 0) + transform.position, Quaternion.identity);
            ob.name = $"{name}'s Worm Node {i}";
            nodes[i] = ob.transform;
            // nodes[i].GetComponent<WormNode>().SetID(i);
            // nodes[i].GetComponent<WormNode>().SetPesudoParent(gameObject);

        }
    }
    private IEnumerator Scan()
    {
        yield return new WaitForSeconds(2f);
        target = FindObjectOfType<RanoScript>().gameObject;
        doneScanning = true;
        yield break;
    }
    // Update is called once per frame
    void Update()
    {
        if (doneScanning && TargetInRange())
        {

            var rb = GetComponent<Rigidbody2D>();
            Vector2 direction = ((Vector2)target.transform.position - rb.position).normalized;

            Vector2 force = direction * speed;

            rb.velocity = force;
            BringFollowerNodes();
        }




    }
    
    //subscribe to death event
    private void DeathSubby()
    {
        //it wasnt really stopping before
        GetComponent<BoxCollider2D>().enabled = false;
        //so fucking metal bro
        FindAnyObjectByType<GameManagerScript>().StartCoroutine(killTheChildrenToo());
    }
    public IEnumerator killTheChildrenToo()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
           
            yield return new WaitForSeconds(.1f);
            nodes[i].gameObject.GetComponent<EntityBaseScript>().die();
            
        }
        yield break;
    }
}
