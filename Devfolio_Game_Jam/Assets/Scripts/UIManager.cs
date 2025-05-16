using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text pondWaterText;
    public TMP_Text populationText;
    public TMP_Text sickText;
    public TMP_Text deadText;
    public TMP_Text moneyText;
    public TMP_Text tankETAText;

    public void UpdateTimer(float time)
    {
        timerText.text = $"Time: {time:F0}";
    }

    public void UpdatePondWater(float water)
    {
        pondWaterText.text = $"Pond: {water:F1}";
    }

    public void UpdatePopulation(int total, int sick, int dead)
    {
        populationText.text = $"Population: {total}";
        sickText.text = $"Sick: {sick}";
        deadText.text = $"Dead: {dead}";
    }

    public void UpdateMoney(int money)
    {
        moneyText.text = $"Money: {money}";
    }

    public void UpdateTankETA(float eta)
    {
        tankETAText.text = $"Tank ETA: {eta:F1}s";
    }
}
