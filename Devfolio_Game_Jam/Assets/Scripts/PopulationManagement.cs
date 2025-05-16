using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class PopulationManagement : MonoBehaviour
{
    public int maxPopulation = 50;
    public int currentPopulation;
    public int currentSick;
    public int currentDead;

    public int deathThreshold = 10; // Set this to your game over threshold

    public GameObject GameFailedUI;

    public LevelManager levelManager;

    public TextMeshProUGUI MaxPopulationUI;
    public TextMeshProUGUI CurrentPopulationUI;
    public TextMeshProUGUI CurrentSickUI;
    public TextMeshProUGUI CurrentDeadUI;

    private void Start()
    {
        currentPopulation = maxPopulation;
        currentSick = 0;
        currentDead = 0;
        GameFailedUI.SetActive(false);
    }

    public void addSick()
    {
        currentSick += 1;
        currentPopulation -= 1;
    }

    public void addDead()
    {
        currentDead += 1;
        currentSick -= 1;
        if (currentSick < 0) currentSick = 0;
    }

    public void addSaved()
    {
        currentPopulation += 1;
        currentSick -= 1;
        if (currentSick < 0) currentSick = 0;
    }

    private void Update()
    {
        MaxPopulationUI.text = maxPopulation.ToString();
        CurrentPopulationUI.text = currentPopulation.ToString();
        CurrentSickUI.text = currentSick.ToString();
        CurrentDeadUI.text = currentDead.ToString();

        if (currentDead >= deathThreshold)
        {
            levelManager.isTimerPaused = true;
            GameFailedUI.SetActive(true);
        }
    }

    public void resetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
