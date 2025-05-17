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
    public TextMeshProUGUI timer;
    public TextMeshProUGUI tankWater;
    public PopulationManagement populationManagement;
    public AreaManagement[] areaManagements;
    public HospitalManager hospitalManagers;
    public int waterTank = 50;
    public float gameSpeed = 0.25f;
    public GameObject pauseMenuUI;

    public TextMeshProUGUI moneyUI;
    public int money = 10;

    public GameObject LevelSummeryIntro;


    private void Start()
    {
        LevelSummeryIntro.SetActive(true);
        // PAUSE GAME AT START
        Time.timeScale = 0f;

        nextLevelName = $"Level {LevelNo + 1}";
        int totalPopulation = populationManagement.maxPopulation;
        int totalArea = areaManagements.Length;

        // Divide the whole population among the 3 areas randomly
        int[] populations = new int[totalArea];
        int remainingPopulation = totalPopulation;
        for (int i = 0; i < totalArea - 1; i++)
        {
            // Ensure at least 1 person per area, and randomize the rest
            int maxForThisArea = remainingPopulation - (totalArea - i - 1);
            populations[i] = Random.Range(1, maxForThisArea + 1);
            remainingPopulation -= populations[i];
        }
        populations[totalArea - 1] = remainingPopulation; // Assign the rest to the last area

        for (int i = 0; i < totalArea; i++)
        {
            areaManagements[i].areaPopulation = populations[i];
            areaManagements[i].waterNeeded = populations[i];
            areaManagements[i].currentWater = 0;
        }

        foreach (var area in areaManagements)
        {
            area.gameSpeed = gameSpeed;
        }
        if (hospitalManagers != null)
            hospitalManagers.gameSpeed = gameSpeed;
    }

    private void Update()
    {

        moneyUI.text = $"Rs {money.ToString()}";

        if (Time.timeScale == 0f) return; // PAUSE ALL LOGIC

        if (!isTimerPaused)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= levelTimer)
            {
                isTimerPaused = true;
                GameCompletedUI.SetActive(true);
            }
        }
        int minutes = Mathf.FloorToInt(currentTime / 60F);
        int seconds = Mathf.FloorToInt(currentTime - minutes * 60);
        string timerText = string.Format("{0:0}:{1:00}", minutes, seconds);
        timer.text = "Time: " + timerText;
        tankWater.text = waterTank.ToString();
    }

    public void StartLevel()
    {
        LevelSummeryIntro.SetActive(false);
        Time.timeScale = 1f;
    }
    public void PauseLevel()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }
    public void ResumeLevel()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public bool TryProvideWater()
    {
        if (waterTank > 0)
        {
            waterTank--;
            return true;
        }
        else
        {
            foreach(AreaManagement pipeScript in areaManagements)
            {
                // set pipe state to false
                pipeScript.pipeManagement.pipeState = false;
            }
            hospitalManagers.pipeManagement.pipeState = false;
        }
        return false;
    }

    public void addMoney(int pay)
    {
        money += pay;
    }

    public void removeMoney(int pay)
    {
        money -= pay;
        if (money < 0)
        {
            isTimerPaused = true;
            populationManagement.GameFailedUI.SetActive(true);
            populationManagement.FailReason.text = "You are out of money!";

        }
    }

}
