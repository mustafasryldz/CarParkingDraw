using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public void ReloadScene()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }

    public void GameStart()
    {
        //int currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(1);
    }
    public void GameQuit()
    {
        Application.Quit();
    }
    public void GameMenu()
    {
        //int currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(0);
    }
}
