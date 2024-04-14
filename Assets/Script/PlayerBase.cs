using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private static PlayerBase instance;

    // Private constructor to prevent instantiation from outside
    private PlayerBase() { }

    // Public static method to get the singleton instance
    public static PlayerBase GetInstance()
    {
        // If the instance hasn't been created yet, create a new one
        if (instance == null)
        {
            instance = new PlayerBase();
        }
        // Return the existing instance
        return instance;
    }


    public float WaterAmount { private set; get; } = 50f;

    public void IncreaseWaterAmount(float increaseAmount) 
    {
        WaterAmount += increaseAmount;
    }

    public void DecreaseWaterAmount(float increaseAmount)
    {
        WaterAmount -= increaseAmount;
    }
}
