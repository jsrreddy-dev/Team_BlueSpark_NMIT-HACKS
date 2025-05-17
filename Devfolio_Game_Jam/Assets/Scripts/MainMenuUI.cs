using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    public GameObject CreditsUI;

    public void StatGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void gameQuit()
    {
        Application.Quit();
    }
    public void OpenCredits()
    {
        CreditsUI.SetActive(true);


    }
    public void CloseCredits()
    {

        CreditsUI.SetActive(false);
    }
}
