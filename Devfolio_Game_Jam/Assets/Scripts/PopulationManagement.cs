using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PopulationManagement : MonoBehaviour
{
    public int maxPopulation = 50;
    public int currentPopulation;
    public int currentSick;
    public int currentDead;

    public int deathThreshold = 10; // Set this to your game over threshold

    public GameObject GameFailedUI;
    public TextMeshProUGUI FailReason;

    public LevelManager levelManager;

    public TextMeshProUGUI MaxPopulationUI;
    public TextMeshProUGUI CurrentPopulationUI;
    public TextMeshProUGUI CurrentSickUI;
    public TextMeshProUGUI CurrentDeadUI;

    public TextMeshProUGUI MaxPopulation;
    public TextMeshProUGUI CurrentPopulation;
    public TextMeshProUGUI CurrentSick;
    public TextMeshProUGUI CurrentDead;
    public RawImage MedalImage;


    public Texture PlatinumMedal;
    public Texture GoldMedal;
    public Texture SilverMedal;
    public Texture BronzeMedal;

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
            FailReason.text = "Game Over! Too many deaths!";
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

        if (currentDead < 5)
        {
            MedalImage.texture = PlatinumMedal;
        }
        else if (currentDead < 10)
        {
            MedalImage.texture = GoldMedal;
        }
        else if (currentDead < 15)
        {
            MedalImage.texture = SilverMedal;
        }
        else
        {
            MedalImage.texture = BronzeMedal;
        }
    }

    public void resetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quit()
    {
        Application.Quit();
    }
}
