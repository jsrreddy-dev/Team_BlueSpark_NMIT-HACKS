using UnityEngine;

public class PipeController : MonoBehaviour
{
    public string destination; // e.g., "Farm", "School"
    public bool isOn = false;
    public float flowRate = 1f; // Units per second

    private WaterManager waterManager;

    void Start()
    {
        // Updated to use the recommended method
        waterManager = Object.FindFirstObjectByType<WaterManager>();
    }

    void Update()
    {
        if (isOn && waterManager != null)
        {
            waterManager.DrainWater(destination, flowRate * Time.deltaTime);
        }
    }

    public void TogglePipe()
    {
        isOn = !isOn;
    }
}
