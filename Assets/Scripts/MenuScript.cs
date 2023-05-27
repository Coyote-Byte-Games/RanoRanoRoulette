using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*

*/

public class MenuScript : MonoBehaviour
{
    enum Scene
    {
        MainMenu,
        Tutorial,
        GL
    }
    // Start is called before the first frame update
    public void PlayGame()
    {
        //SceneManager.GetActiveScene().buildIndex + 1
        SceneManager.LoadScene(((int)Scene.GL));
    }
   
       public void MainMenu()
    {
        //SceneManager.GetActiveScene().buildIndex + 1
        SceneManager.LoadScene(((int)Scene.MainMenu));
    }
    public void Tutorial()
    {
        SceneManager.LoadScene(((int)Scene.Tutorial));
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
