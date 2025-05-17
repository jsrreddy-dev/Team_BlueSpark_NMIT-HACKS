using UnityEngine;
using TMPro;

public class AreaManagement : MonoBehaviour
{
    public int areaPopulation;
    public int waterNeeded;
    public int currentWater;

    public HospitalManager hospitalManager;
    public PipeManagement pipeManagement;
    public LevelManager levelManager;

    public int sickPerUpdate = 1;

    [Header("Game Speed Control")]
    public float gameSpeed = 1f; // 1 = normal, <1 = slower, >1 = faster

    [Header("Timing (in seconds, before speed adjustment)")]
    public float baseWaterInterval = 1f; // 3 seconds to fill 1 unit
    public float baseCheckInterval = 5f; // 5 seconds for sickness check

    private float waterTimer = 0f;
    private float checkTimer = 0f;
    private float cunsumptionTimer = 0f;
    float waterCounsumInterval = 10f;

    public int areaIndex;
    public TextMeshProUGUI currentWaterUI;

    private void Start()
    {
        waterNeeded = areaPopulation / 3;
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        currentWaterUI.text = $"Current Water: {currentWater}";

        float waterInterval = baseWaterInterval;
        float checkInterval = baseCheckInterval / gameSpeed;

        // Water receiving logic
        if (pipeManagement != null && pipeManagement.pipeState)
        {
            waterTimer += Time.deltaTime;
            if (waterTimer >= waterInterval)
            {
                if (levelManager != null && levelManager.TryProvideWater())
                {
                    currentWater += 1;
                    levelManager.addMoney(5);
                }
                waterTimer = 0f;
            }
        }
        else
        {
            waterTimer = 0f;
        }

        // Sickness/water check
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            if (currentWater < waterNeeded && areaPopulation > 0)
            {
                int sick = Mathf.Min(sickPerUpdate, areaPopulation);
                areaPopulation -= sick;
                hospitalManager.AdmitSickPeople(sick, areaIndex);
                if (levelManager != null && levelManager.populationManagement != null)
                {
                    for (int i = 0; i < sick; i++)
                        levelManager.populationManagement.addSick();
                }
            }
            checkTimer = 0f;
        }

        // Water consumption logic
        cunsumptionTimer += Time.deltaTime;
        if (cunsumptionTimer >= waterCounsumInterval)
        {
            if (currentWater > 0)
            {
                currentWater -= 1;
            }
            cunsumptionTimer = 0f;
        }
    }

    public void ReturnRecoveredPeople(int count)
    {
        areaPopulation += count;
    }
}
