using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int LevelNo;
    public int minWater = 50;
    public float levelTimer; // in seconds
    public float currentTime;
    public bool isTimerPaused = false;
    string nextLevelName;

    public GameObject GameCompletedUI;
    public int currentPopulation;
    public int maxPopulation;
    public int currentSick;
    public int currentDead;
    public string levelMedal;
    public TextMeshProUGUI MaxPopulation;
    public TextMeshProUGUI CurrentPopulation;
    public TextMeshProUGUI CurrentSick;
    public TextMeshProUGUI CurrentDead;
    public TextMeshProUGUI LevelMedal;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI tankWater;
    public PopulationManagement populationManagement;
    public AreaManagement[] areaManagements;
    public HospitalManager hospitalManagers;
    public int waterTank = 50;

    private void Start()
    {
        nextLevelName = $"Level {LevelNo + 1}";
        int totalPopulation = populationManagement.maxPopulation;
        int totalArea = areaManagements.Length;
        int totalHospital = 1;
        int total = totalArea + totalHospital;
        int[] populations = new int[total];
        int remainingPopulation = totalPopulation;
        for (int i = 0; i < total; i++)
        {
            populations[i] = Random.Range(1, remainingPopulation / (total - i));
            remainingPopulation -= populations[i];
        }
        for (int i = 0; i < totalArea; i++)
        {
            areaManagements[i].areaPopulation = populations[i];
            areaManagements[i].waterNeeded = populations[i];
            areaManagements[i].currentWater = 0;
        }
    }

    private void Update()
    {
        if (!isTimerPaused)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= levelTimer)
            {
                isTimerPaused = true;
                updateGameOverUI();
                GameCompletedUI.SetActive(true);
            }
        }
        int minutes = Mathf.FloorToInt(currentTime / 60F);
        int seconds = Mathf.FloorToInt(currentTime - minutes * 60);
        string timerText = string.Format("{0:0}:{1:00}", minutes, seconds);
        timer.text = "Time: " + timerText;
        tankWater.text = waterTank.ToString();
    }

    void updateGameOverUI()
    {
        currentDead = populationManagement.currentDead;
        currentSick = populationManagement.currentSick;
        currentPopulation = populationManagement.currentPopulation;
        maxPopulation = populationManagement.maxPopulation;

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

        MaxPopulation.text = "Max Population: " + maxPopulation.ToString();
        CurrentPopulation.text = "Current Population: " + currentPopulation.ToString();
        CurrentSick.text = "Current Sick: " + currentSick.ToString();
        CurrentDead.text = "Current Dead: " + currentDead.ToString();
        LevelMedal.text = "Level Medal: " + levelMedal.ToString();
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    public bool TryProvideWater()
    {
        if (waterTank > 0)
        {
            waterTank--;
            return true;
        }
        return false;
    }
}
