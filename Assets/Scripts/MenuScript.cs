using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*

*/

public class MenuScript : MonoBehaviour
{
   
    // public OnDestroy()
    // {

    // }
    public GameObject fadeGO;
   public static MenuScript instance;
   public GameConfig config;
   public bool usingMouse = true;
    //singleton
      private void Awake()
    {
        Debug.Log("awake called on the thingy");
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    public void PlayGame()
    {
        //SceneManager.GetActiveScene().buildIndex + 1
        // SceneManager.LoadScene(((int)SceneEnum.GL));
        config.currentLevel = 0;
        StartCoroutine(LoadGameScene((int)SceneEnum.GL));
    }
     public IEnumerator LoadGameScene(int index)
    {
        for (; ; )
        {
            // yield return new WaitForSeconds(.1f);
            fadeGO.GetComponent<Animator>().SetTrigger("GameOver");
            yield return new WaitForSeconds(3.2f);
            SceneManager.LoadScene(index);
            yield return new WaitForSeconds(1.5f);
            // gameOverFade.GetComponent<Animator>().SetTrigger("Open Back Up");
            // yield return new WaitForSeconds(1.75f);

            yield break;
            
        }
    }
    //  public void SetUsingMouse(bool input)
    // {
    //     //SceneManager.GetActiveScene().buildIndex + 1
    //     config.usingMouse = input;
      
    // }
    ///<summary>
    ///Loads a scene with a transition. 
    ///</summary>
    ///
    public void LoadSceneSoSoSoftly(SceneEnum sceneToLoad)
    {
        StartCoroutine(LoadSceneSoftly(sceneToLoad));
    }
    private IEnumerator LoadSceneSoftly(SceneEnum toLoad)
    {
        //I DID IT
        //I FOUND A GOOD WAY TO USE ENUMS
        //FUCK YOU BILL GATES IM NOT GONNA DIE IN THIS CESSPOOL OF YOUR TWISTED DESIGN
        for (; ; )
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(((int)toLoad));
        yield break;

        }
    }
    public void MainMenu()
    {
        //SceneManager.GetActiveScene().buildIndex + 1
        SceneManager.LoadScene(((int)SceneEnum.MainMenu));
    }
    public void Tutorial()
    {
        SceneManager.LoadScene(((int)SceneEnum.Tutorial));
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
public enum SceneEnum
{
    MainMenu,
    Tutorial,
    GL,
    GAMEOVER
}