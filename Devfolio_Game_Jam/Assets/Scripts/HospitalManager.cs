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

    private float waterTimer = 0f;
    private const float waterInterval = 1f;

    public float checkInterval = 5f;
    private float checkTimer = 0f;

    public TextMeshProUGUI debug;

    private Queue<int> patientAreaQueue = new Queue<int>();

    private void Update()
    {
        debug.text = $"Hospital Population: {hospitalPopulation}\n" +
            $"Water Needed: {waterNeeded}\n" +
            $"Current Water: {currentWater}\n" +
            $"Pipe State: {pipeManagement.pipeState}";

        // Water receiving logic
        if (pipeManagement.pipeState)
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

        // Sickness/death/recovery check
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            // Deaths if not enough water
            if (currentWater < waterNeeded && hospitalPopulation > 0)
            {
                int deaths = Mathf.Min(treatPerUpdate, hospitalPopulation, patientAreaQueue.Count);
                for (int i = 0; i < deaths; i++)
                {
                    patientAreaQueue.Dequeue();
                    if (levelManager != null && levelManager.populationManagement != null)
                        levelManager.populationManagement.addDead();
                }
                hospitalPopulation -= deaths;
                currentWater = 0; // Not enough water, reset
            }
            // Recovery if enough water
            else if (currentWater >= waterNeeded && hospitalPopulation > 0 && patientAreaQueue.Count > 0)
            {
                int treated = Mathf.Min(treatPerUpdate, hospitalPopulation, patientAreaQueue.Count);
                for (int i = 0; i < treated; i++)
                {
                    int areaIndex = patientAreaQueue.Dequeue();
                    if (areaIndex >= 0 && areaIndex < areaManagements.Length)
                        areaManagements[areaIndex].ReturnRecoveredPeople(1);
                    if (levelManager != null && levelManager.populationManagement != null)
                        levelManager.populationManagement.addSaved();
                }
                hospitalPopulation -= treated;
                currentWater -= waterNeeded * treated;
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
