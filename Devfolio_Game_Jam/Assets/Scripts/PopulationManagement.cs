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

    public TextMeshProUGUI MaxPopulation;
    public TextMeshProUGUI CurrentPopulation;
    public TextMeshProUGUI CurrentSick;
    public TextMeshProUGUI CurrentDead;
    public TextMeshProUGUI LevelMedal;

    public string levelMedal;

    private void Start()
    {
        currentPopulation = maxPopulation;
        currentSick = 0;
        currentDead = 0;
        GameFailedUI.SetActive(false);
    }
    private void Update()
    {

        if (Time.timeScale == 0f) return;

        MaxPopulationUI.text = maxPopulation.ToString();
        CurrentPopulationUI.text = currentPopulation.ToString();
        CurrentSickUI.text = currentSick.ToString();
        CurrentDeadUI.text = currentDead.ToString();

        updateGameOverUI();

        if (currentDead >= deathThreshold)
        {
            levelManager.isTimerPaused = true;
            GameFailedUI.SetActive(true);
        }
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

    void updateGameOverUI()
    {

        MaxPopulation.text = "Max Population: " + maxPopulation.ToString();
        CurrentPopulation.text = "Current Population: " + currentPopulation.ToString();
        CurrentSick.text = "Current Sick: " + currentSick.ToString();
        CurrentDead.text = "Current Dead: " + currentDead.ToString();
        LevelMedal.text = "Level Medal: " + levelMedal.ToString();

        if (currentDead == 0)
            levelMedal = "Platinum";
        else if (currentDead <= 2)
            levelMedal = "Gold";
        else if (currentDead <= 5)
            levelMedal = "Silver";
        else if (currentDead <= 10)
            levelMedal = "Bronze";
        else
            levelMedal = "No Medal";
    }

   

    public void resetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
