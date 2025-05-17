using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class waterRefillManager : MonoBehaviour
{

    public GameObject waterTank;
    public int tankCapacity;
    public float tankTimetoRefill;
    public Vector3 stationLoc;
    public Button refillButton;

    public LevelManager levelManager;

    bool isRefilling = false;

    private void Start()
    {
        refillButton.interactable = true;
    }

    public void sendForRefill()
    {
        if (waterTank != null && !isRefilling)
        {
            StartCoroutine(refillWaterfromStation());
        }
    }

    IEnumerator refillWaterfromStation()
    {
        isRefilling = true;
        refillButton.interactable = false; // Disable the button to prevent multiple clicks
        levelManager.removeMoney(30);
        float elapsedTime = 0f;
        Vector3 startPos = waterTank.transform.position;
        Vector3 endPos = stationLoc;
        float halfTime = tankTimetoRefill / 2f;

        // Move to station
        while (elapsedTime < halfTime)
        {
            waterTank.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / halfTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        waterTank.transform.position = endPos;

        // Wait at station (optional, can adjust time if needed)
        // yield return new WaitForSeconds(0.1f);

        // Move back to original position
        float returnElapsed = 0f;
        while (returnElapsed < halfTime)
        {
            waterTank.transform.position = Vector3.Lerp(endPos, startPos, returnElapsed / halfTime);
            returnElapsed += Time.deltaTime;
            yield return null;
        }
        waterTank.transform.position = startPos;

        // Only refill once and reset state
        levelManager.waterTank += tankCapacity;
        refillButton.interactable = true; // Re-enable the button
        isRefilling = false;
    }


}
