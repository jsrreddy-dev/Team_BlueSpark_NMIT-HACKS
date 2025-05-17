using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HospitalManager : MonoBehaviour
{
    public int waterNeeded;
    public int currentWater;
    public int hospitalPopulation;

    public PipeManagement pipeManagement;
    public AreaManagement[] areaManagements;
    public LevelManager levelManager;

    public int treatPerUpdate = 1;

    [Header("Game Speed Control")]
    public float gameSpeed = 1f; // 1 = normal, <1 = slower, >1 = faster

    [Header("Timing (in seconds, before speed adjustment)")]
    public float baseWaterInterval = 3f; // 3 seconds to fill 1 unit
    public float baseCheckInterval = 5f; // 5 seconds for sickness/death check

    private float waterTimer = 0f;
    private float checkTimer = 0f;

    public TextMeshProUGUI currentWaterUI;

    private Queue<int> patientAreaQueue = new Queue<int>();

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        currentWaterUI.text = $"Current Water: {currentWater}";

        float waterInterval = baseWaterInterval;
        float checkInterval = baseCheckInterval / gameSpeed;

        // Water needed is based on how many patients are in the hospital
        waterNeeded = hospitalPopulation;

        // Water receiving logic
        if (pipeManagement.pipeState)
        {
            waterTimer += Time.deltaTime;
            if (waterTimer >= waterInterval)
            {
                if (levelManager != null && levelManager.TryProvideWater())
                {
                    currentWater += 1;
                    levelManager.addMoney(10);
                }
                waterTimer = 0f;
            }
        }
        else
        {
            waterTimer = 0f;
        }

        // Sickness/death/recovery check every checkInterval seconds (adjusted by game speed)
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            if (hospitalPopulation > 0 && patientAreaQueue.Count > 0)
            {
                int canTreat = Mathf.Min(treatPerUpdate, hospitalPopulation, patientAreaQueue.Count);

                // If enough water for all being treated, cure them
                if (currentWater >= canTreat && canTreat > 0)
                {
                    for (int i = 0; i < canTreat; i++)
                    {
                        int areaIndex = patientAreaQueue.Dequeue();
                        if (areaIndex >= 0 && areaIndex < areaManagements.Length)
                        {
                            areaManagements[areaIndex].ReturnRecoveredPeople(1);
                            levelManager.addMoney(2);
                        }
                        if (levelManager != null && levelManager.populationManagement != null)
                        {
                            levelManager.populationManagement.addSaved();
                        }
                    }
                    hospitalPopulation -= canTreat;
                    currentWater -= canTreat;
                }
                // If not enough water, people die
                else if (canTreat > 0 && currentWater <= 0)
                {
                    currentWater = 0;
                    for (int i = 0; i < canTreat; i++)
                    {
                        patientAreaQueue.Dequeue();
                        if (levelManager != null && levelManager.populationManagement != null)
                        {
                            levelManager.populationManagement.addDead();
                            levelManager.addMoney(-5);
                        }
                    }
                    hospitalPopulation -= canTreat;
                }
            }
            checkTimer = 0f;
        }
    }

    public void AdmitSickPeople(int count, int areaIndex)
    {
        hospitalPopulation += count;
        for (int i = 0; i < count; i++)
            patientAreaQueue.Enqueue(areaIndex);
    }
}
