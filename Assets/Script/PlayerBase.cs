using UnityEngine;

public class PlayerBase : MonoBehaviour
{
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
