using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    
    public void StatGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void gameQuit()
    {
        Application.Quit();
    }

}
