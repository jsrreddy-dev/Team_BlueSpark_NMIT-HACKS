using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public float pondCapacity = 50f;
    public float currentWater;
    public Dictionary<string, float> locationNeeds = new();
    public float refillDelay = 5f;
    public float tankAmount = 10f;
    public bool isRefilling = false;

    void Start()
    {
        currentWater = pondCapacity;
        locationNeeds["Farm"] = 0f;
        locationNeeds["School"] = 0f;
        locationNeeds["Houses"] = 0f;
        locationNeeds["Hospital"] = 0f;
    }

    public void DrainWater(string location, float amount)
    {
        if (currentWater <= 0f) return;
        float drained = Mathf.Min(amount, currentWater);
        currentWater -= drained;
        locationNeeds[location] += drained;
    }

    public void RequestRefill()
    {
        if (!isRefilling)
            StartCoroutine(RefillTankCoroutine());
    }

    private System.Collections.IEnumerator RefillTankCoroutine()
    {
        isRefilling = true;
        yield return new WaitForSeconds(refillDelay);
        currentWater = Mathf.Min(currentWater + tankAmount, pondCapacity);
        isRefilling = false;
    }
}
