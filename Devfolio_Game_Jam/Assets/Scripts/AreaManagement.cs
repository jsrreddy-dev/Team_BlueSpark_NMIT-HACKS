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

    private float waterTimer = 0f;
    private const float waterInterval = 1f;

    public float checkInterval = 5f;
    private float checkTimer = 0f;

    public int areaIndex;
    public TextMeshProUGUI debug;

    private void Update()
    {
        debug.text = $"Area Population: {areaPopulation}\n" +
            $"Water Needed: {waterNeeded}\n" +
            $"Current Water: {currentWater}\n" +
            $"Pipe State: {pipeManagement.pipeState}";

        // Water receiving logic
        if (pipeManagement != null && pipeManagement.pipeState)
        {
            waterTimer += Time.deltaTime;
            if (waterTimer >= waterInterval)
            {
                if (levelManager != null && levelManager.TryProvideWater())
                {
                    currentWater += 1;
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
                currentWater = 0; // Not enough water, reset
            }
            else if (currentWater >= waterNeeded)
            {
                currentWater -= waterNeeded; // Only subtract what was needed, keep the rest
            }
            checkTimer = 0f;
        }
    }

    public void ReturnRecoveredPeople(int count)
    {
        areaPopulation += count;
    }
}
