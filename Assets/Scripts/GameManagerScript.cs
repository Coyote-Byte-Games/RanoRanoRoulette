using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
public class GameManagerScript : MonoBehaviour
{//TODO Migrate UI functions to UIManager
public string modTimeMessage  = "Time until New Mod";
    private UIManagerScript uiManager;
    public GameObject FlagEndpoint;
    ModifierManager modMan;
    public GameObject portal;
    public GameObject rano;
    public GameObject TMProModTimeRemaining;
    public GameObject testEnemy;
    public LevelGenerator LevelGenerator;
    public int numberOfChunks;
    public Image gameOverFade;
    [SerializeField]

    [HideInInspector]
    private float delayTimeElapsed;

    public RuleTile currentLevelTile;
    public Tile bgTile;
    public Tile[] garnishTiles;
    public Tilemap Tilemap;
    public Tilemap bgTilemap;
public AudioSource audioSource;

    
    public Texture2D[] sliceTextures;
    [UnityEngine.Header("Level Generation")]
    public GameObject[] levelTraps; 
    private void UpdateModRemainingTime(TextMeshProUGUI element, float timeRemaining,  string defaultTextVal = "Time until new mod:")
    {
      
      element.text = $"{defaultTextVal} {timeRemaining.ToString("F1")}";
    }
  
    public IEnumerator CreateRano()
    {
        yield return new WaitForSeconds(1);
          var port = Instantiate(portal, new Vector3(-10,5,0) ,Quaternion.identity);
        yield return new WaitForSeconds(1);
        rano.SetActive(true);
        rano.transform.position = new Vector3(-10,5,0);
        rano.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -10000));
        yield return new WaitForSeconds(.75f);
        Destroy(port);
        

          yield break;
    }


    public void SpawnEnemy(int x, int y)
    {
        try
        {
             Instantiate(testEnemy, new Vector3(x,y,0) ,Quaternion.identity);
        }
        catch (System.Exception e)
        {
            
            throw e;
        }
       
    }
    void GetCurrentLevelTheme()
    {

    }
    public void GameOver()
    {

        StartCoroutine(nameof(LoadGameOverScene));
    }
    public IEnumerator LoadGameOverScene()
    {
        for (;;)
        {
            yield return new WaitForSeconds(2f);
        gameOverFade.GetComponent<Animator>().SetTrigger("GameOver");
            yield return new WaitForSeconds(1.75f);
            SceneManager.LoadScene("GameOverScene");
        }
    }


    public bool startUpDelayFinished = false;
    public GameData data;
    public GameObject WheelPrefab;
    public int modifierInterval;
    public GameObject player;
    // public GameObject wheel;
    private WheelScript wheelScript;
    float timeElapsed;
    //the time it takes before mods begin dispersal.
    public float modStartupTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(CreateRano));
        LevelGenerator.manager = this;
        wheelScript = WheelPrefab.GetComponent<WheelScript>();
        StartCoroutine(nameof(BeginNewMod));

    }

    private IEnumerator BeginNewMod()
    {
        for (; ; )
        {
            
            for (delayTimeElapsed = 0; delayTimeElapsed < modStartupTime; delayTimeElapsed += Time.deltaTime)
            {
                yield return null;
            }
           
            // LaunchNewModifier();
            startUpDelayFinished = true;
            Debug.Log("This message should never appear twice. If it does, there is a bug in modifier generation.");
            yield break;
        }

    }
    void OnEnable()
    {
        // modMan.AssignModToggles(data.inspectorModToggles);

    }
    void Awake()
    {
        modMan = new ModifierManager();

        data.mods = modMan.GenerateRandomMods(data.numOfMods);
        // Debug.Log(data.mods[0]);
        // Debug.Log(Color.white.ToString("F2"));
        

        LevelGenerator = new LevelGenerator(this, Tilemap, bgTilemap, currentLevelTile, bgTile, garnishTiles, sliceTextures, levelTraps);
        LevelGenerator.flag = FlagEndpoint;
        LevelGenerator.GenerateLevelChunksWithEndpoint(numberOfChunks);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateModRemainingTime(TMProModTimeRemaining.GetComponent<TextMeshProUGUI>(), GetTimeUntilNewMod() + (modStartupTime - delayTimeElapsed), modTimeMessage);

        //checking if we need a new modifier
        if (startUpDelayFinished)
        {
            timeElapsed += Time.deltaTime;
        }
        if (timeElapsed >= modifierInterval)
        {
            timeElapsed = 0;
            LaunchNewModifier();

        }

    }

    private float GetTimeUntilNewMod()
    {

       return startUpDelayFinished?  modifierInterval - timeElapsed : (modifierInterval - timeElapsed);//make this return the time truly waited when were hung on the thing
    }

    //: Generates a new modifier. Includes the entire phase of spawning the wheel, spinning, choosing the modifier and ending.
    void LaunchNewModifier()
    {
        //if out of mods
        if (data.numOfMods < 1)
        {
            // Debug.Log("NO MORE MODS FOR YOU AAHAHAHAHAHAHAAAAAA  there wer e none to begin with.");
            modTimeMessage = "Remaining bananas: ";
            return;
        }
        var wheelInstance = Instantiate(WheelPrefab, Vector3.zero, Quaternion.identity);
        Destroy(wheelInstance, 3);
        IModifier newMod = wheelScript.Launch();
        //:the modifier is null at this point
        player.GetComponent<RanoScript>().AddModifier(newMod);

        data.numOfMods--;
        // i am having a stroke


    }

    internal void RunCutscene(Cutscene cutID)
    {
        // switch (cutID)
        // {
        //     case Cutscene.LEVEL_VICTORY:
        //     default: throw new NotImplementedException();
        // }
        GameOver();
    }
}
//todo create new comments, docs, debug? explain, function?
