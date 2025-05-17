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

        RectTransform tankRect = waterTank.GetComponent<RectTransform>();
        bool hasFlippedToStation = false;
        bool hasFlippedToStart = false;

        // Move to station
        while (elapsedTime < halfTime)
        {
            waterTank.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / halfTime);
            elapsedTime += Time.deltaTime;

            // Flip to face station when reaching station
            if (!hasFlippedToStation && elapsedTime >= halfTime)
            {
                if (tankRect != null)
                    tankRect.localScale = new Vector3(-Mathf.Abs(tankRect.localScale.x), tankRect.localScale.y, tankRect.localScale.z);
                hasFlippedToStation = true;
            }

            yield return null;
        }
        waterTank.transform.position = endPos;
        if (tankRect != null)
            tankRect.localScale = new Vector3(-Mathf.Abs(tankRect.localScale.x), tankRect.localScale.y, tankRect.localScale.z);

        // Wait at station (optional, can adjust time if needed)
        // yield return new WaitForSeconds(0.1f);

        // Move back to original position
        float returnElapsed = 0f;

        while (returnElapsed < halfTime)
        {
            waterTank.transform.position = Vector3.Lerp(endPos, startPos, returnElapsed / halfTime);
            returnElapsed += Time.deltaTime;

            // Flip back to face start when reaching start
            if (!hasFlippedToStart && returnElapsed >= halfTime)
            {
                if (tankRect != null)
                    tankRect.localScale = new Vector3(Mathf.Abs(tankRect.localScale.x), tankRect.localScale.y, tankRect.localScale.z);
                hasFlippedToStart = true;
            }

            yield return null;
        }
        waterTank.transform.position = startPos;
        if (tankRect != null)
            tankRect.localScale = new Vector3(Mathf.Abs(tankRect.localScale.x), tankRect.localScale.y, tankRect.localScale.z);

        // Only refill once and reset state
        levelManager.waterTank += tankCapacity;
        refillButton.interactable = true; // Re-enable the button
        isRefilling = false;
    }


}
