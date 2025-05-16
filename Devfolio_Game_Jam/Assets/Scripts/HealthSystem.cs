using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int totalPopulation = 100;
    public int sick = 0;
    public int dead = 0;
    public int gameOverThreshold = 15;

    private Dictionary<string, float> lastSupplied = new();

    void Start()
    {
        lastSupplied["Farm"] = 0f;
        lastSupplied["School"] = 0f;
        lastSupplied["Houses"] = 0f;
        lastSupplied["Hospital"] = 0f;
    }

    public void UpdateHealth(string location, float supplied)
    {
        if (supplied < 1f) // Example threshold
        {
            sick++;
            if (location == "Hospital")
            {
                dead++;
            }
        }
        if (dead > gameOverThreshold)
        {
            // Trigger game over
        }
    }
}
