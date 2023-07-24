using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
public class GameManagerScript : MonoBehaviour
{//TODO Migrate UI functions to UIManager

    public LevelManagerScript[] levels;
    public GameObject gridLevelParent;
    //2:11AM coding makes the best "it works" garbage
    public GameObject fadeGO;
    public SFXManagerSO sfxMan;
    public AudioClip wheelSFX;
    private bool _creatingRano;
    public GameObject menuScript;
    public string modTimeMessage = "Time until New Mod";
    [Space]
    public ModifierManager modMan;
    public GameConfig config;
    private UIManagerScript uiManager;
    // public LevelGenerator LevelGenerator;
    public GameObject bgImage;
    //  public LevelSlice[] sliceTextures;
    [Space]
    // public GameObject FlagEndpoint;
    public GameObject portal;
    public GameObject rano;
    public GameObject TMProModTimeRemaining;


    [Space]
    public Image gameOverFade;
    public Vector3 defaultSpawnLocation;
    [Space]
    public Tilemap Tilemap;
    public Tilemap bgTilemap;
    // public RuleTile currentLevelTile;
    // public Tile[] garnishTiles;
    // public Tile bgTile;

    [Space]
    public int numberOfChunks;
    private float delayTimeElapsed;
    public AudioSource audioSource;

    [UnityEngine.Header("Level Generation")]
    public GameObject[] levelTraps;
    public LevelManagerScript levelManager;
    public GameObject button;
    public GameObject wall;
    public bool looping;



    private void UpdateModRemainingTime(TextMeshProUGUI element, float timeRemaining, string text = "Time until new mod:")
    {
        string strong;
        if (text == string.Empty)
        {
            strong = text;

        }
        else
        {
            strong = $"{text} {timeRemaining.ToString("F1")}";

        }
        element.text = strong;
    }
    public void CleanupForLevelReload(bool changingLevel = false)
    {
        modMan.Reset();
        rano.GetComponent<EntityBaseScript>().Health = rano.GetComponent<EntityBaseScript>().maxHP;
        if (changingLevel)
        {
            GameConfig.seed++;

        }
    }

    public IEnumerator CreateRano(Vector3 spawnLocation)
    {
        yield return new WaitForSeconds(.25f);

        if (_creatingRano)
        {
            yield break;
        }
        _creatingRano = true;

        rano.transform.position = spawnLocation;

        rano.GetComponentInChildren<SpriteRenderer>().enabled = false;
        rano.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        var port = Instantiate(portal, spawnLocation, Quaternion.identity);
        audioSource.PlayOneShot(sfxMan.GetClip(SFXManagerSO.Sound.portalOpen));
        yield return new WaitForSeconds(1);
        rano.SetActive(true);
        rano.GetComponentInChildren<SpriteRenderer>().enabled = true;
        rano.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;



        rano.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -10000));
        yield return new WaitForSeconds(.75f);
        Destroy(port);

        _creatingRano = false;
        yield break;
    }


    public void LoadNextLevel()
    {
        if ((++config.currentLevel >= levels.Length))
        {
            Debug.Log("bababooey");
            StartCoroutine(LoadSceneStylin(4));
        }
        else
        {
            CleanupForLevelReload(changingLevel:true);
            StartCoroutine(LoadSceneStylin(((int)SceneEnum.GL)));

        }

    }

    public void SpawnButton(float x, float y, GameObject parent)
    {
        try
        {
            var buttonInstance = Instantiate(button, new Vector3(x, y, 0), Quaternion.identity, parent.transform);
            //use the parent gameobject to parent this and any other trap instances to the same parent object
        }
        catch (System.Exception e)
        {

            throw e;
        }

    }



    public void SpawnWall(float x, float y, GameObject parent)
    {
        try
        {
            var wallInstance = Instantiate(wall, new Vector3(x, y, 0), Quaternion.identity, parent.transform);
            // wallInstance.transform.SetParent(parent.transform);
        }

        catch (System.Exception e)
        {

            throw e;
        }

    }
    //doing this for the unityEvent
    void GetCurrentLevelTheme()
    {

    }

    public IEnumerator LoadSceneStylin(int toLoad)
    {
        for (; ; )
        {
            // yield return new WaitForSeconds(.1f);
            fadeGO.GetComponent<Animator>().SetTrigger("GameOver");
            yield return new WaitForSeconds(2.25f);
            SceneManager.LoadScene(toLoad);
            yield return new WaitForSeconds(1.5f);
            // gameOverFade.GetComponent<Animator>().SetTrigger("Open Back Up");
            // yield return new WaitForSeconds(1.75f);

            yield break;

        }
    }



    public GameData data;
    public GameObject WheelPrefab;

    public GameObject player;
    // public GameObject wheel;
    private WheelScript wheelScript;
    float timeElapsed;
    //the time it takes before mods begin dispersal.
    public float modStartupTime;

    // Start is called before the first frame update
    void Start()
    {
        //To use instantiate
        // InstantiateUE.AddListener(( GameObject go, Vector2 pos) => Instantiate(go, pos, Quaternion.identity));



        StartCoroutine((CreateRano(defaultSpawnLocation)));


        wheelScript = WheelPrefab.GetComponent<WheelScript>();
        StartCoroutine(nameof(BeginNewMod));

        bgImage.GetComponent<SpriteRenderer>().sprite = levelManager.bgImage;
        bgImage.GetComponent<SpriteRenderer>().color = levelManager.bgColor + new Color(0, 0, 0, 1);
        bgImage.transform.localScale = levelManager.bgScale;
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
            modMan.startUpDelayFinished = true;
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
        // if (config.currentLevel >= levels.Length)
        // {
        //     config.currentLevel = 0;
        // }
        modMan.numOfMods = modMan.startingModNumber;

        levelManager = levels[config.currentLevel];
        Debug.Log("Seed " + GameConfig.GetSeed());
        UnityEngine.Random.InitState(GameConfig.GetSeed());


        modMan.PrepareMods();

        //setting the scene deps of the levelgenerator
        // levelManager.generator.Tilemap = Tilemap;
        // levelManager.generator.bgTilemap = bgTilemap;
        levelManager.generator.gameManager = this;
        // levelManager.generator.WipeTileMap();
        levelManager.generator.GenerateLevelChunks(gridLevelParent);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateModRemainingTime(TMProModTimeRemaining.GetComponent<TextMeshProUGUI>(), GetTimeUntilNewMod() + (modStartupTime - delayTimeElapsed), modTimeMessage);
        Debug.Log("seed " + GameConfig.GetSeed());

        //checking if we need a new modifier
        if (modMan.startUpDelayFinished)
        {
            timeElapsed += Time.deltaTime;
        }
        if (timeElapsed >= modMan.modifierInterval)
        {
            timeElapsed = 0;
            LaunchNewModifier();

        }



    }

    private float GetTimeUntilNewMod()
    {

        return modMan.startUpDelayFinished ? modMan.modifierInterval - timeElapsed : (modMan.modifierInterval - timeElapsed);//make this return the time truly waited when were hung on the thing
    }

    //: Generates a new modifier. Includes the entire phase of spawning the wheel, spinning, choosing the modifier and ending.
    void LaunchNewModifier()
    {
        //if out of mods
        if (modMan.GetNumOfMods() < 1)
        {
            modTimeMessage = string.Empty;
            return;
        }
        FindObjectOfType<AudioSource>().PlayOneShot(wheelSFX);
        var wheelInstance = Instantiate(WheelPrefab, Vector3.zero, Quaternion.identity);

        // wheelInstance.iconParam = 
        Destroy(wheelInstance, 3);
        IModifier newMod = wheelScript.Launch();

        //:the modifier is null at this point
        player.GetComponent<RanoScript>().AddModifier(newMod);

        modMan.numOfMods--;
        // i am having a stroke


    }
}
//todo create new comments, docs, debug? explain, function?
