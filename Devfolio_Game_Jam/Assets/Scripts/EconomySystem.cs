using UnityEngine;

public class EconomySystem : MonoBehaviour
{
    public int money = 0;
    public int waterTankCost = 10;

    public void EarnMoney(int amount)
    {
        money += amount;
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false;
    }
}
