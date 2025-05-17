using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class tankUpgrade : MonoBehaviour
{
    
    public waterRefillManager waterRefillManager;
    public LevelManager levelManager;

    public GameObject UpgradeUI;
    public Button upgradeButton;

    public TextMeshProUGUI cpacityLevelUI;
    public TextMeshProUGUI refillTimeLevelUI;

    public TextMeshProUGUI cpacityInfo;
    public TextMeshProUGUI refillTimeInfo;

    int Capacity = 5;
    int CapacityUpgrade = 3;
    int Capacityprice = 200;

    int Time = 20;
    int TimeUpgrade = 2;
    int Timeprice = 200;

    int CapacityLevel = 1;
    int TimeLevel = 1;

    private void Update()
    {
        cpacityInfo.text = $"Capacity: {Capacity}\r\nUpgrade: +{CapacityUpgrade}\r\nPrice: Rs. {Capacityprice}";
        refillTimeInfo.text = $"Refill Time: {Time}\r\nUpgrade: -{TimeUpgrade}\r\nPrice: Rs. {Timeprice}";
    }

    public void OpneUpgradeMenu()
    {
        UpgradeUI.SetActive(true);
        upgradeButton.interactable = false;

    }

    public void CloseUpgradeMenu()
    {
        UpgradeUI.SetActive(false);
        upgradeButton.interactable = true;
    }

    public void updateCapacityInfo()
    {
        Capacity += CapacityUpgrade;
        Capacityprice += 100;
    }

    public void updateTimeInfo()
    {
        Time -= TimeUpgrade;
        Timeprice += 100;
    }

    public void UpgradeCapacity()
    {
        if (levelManager.money >= Capacityprice)
        {
            levelManager.removeMoney(Capacityprice);
            updateCapacityInfo();
            CapacityLevel++;
            cpacityLevelUI.text = $"Lv: {CapacityLevel}";
            waterRefillManager.tankCapacity = Capacity;
        }
    }
    public void UpgradeTime()
    {
        if (levelManager.money >= Timeprice)
        {
            levelManager.removeMoney(Timeprice);
            updateTimeInfo();
            TimeLevel++;
            refillTimeLevelUI.text = $"Lv: {TimeLevel}";
            waterRefillManager.tankTimetoRefill = Time;
        }
    }


}
