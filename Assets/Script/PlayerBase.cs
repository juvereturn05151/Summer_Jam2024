using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private static PlayerBase instance;

    // Public property to access the singleton instance
    public static PlayerBase Instance
    {
        get
        {
            // If the instance doesn't exist, try to find it in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerBase>();

                // If it still doesn't exist, create a new GameObject with the SingletonExample component
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("PlayerBase");
                    instance = singletonObject.AddComponent<PlayerBase>();
                }

                // Ensure the instance persists between scene changes
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    // Optional Awake method to ensure the instance is created before any other script's Start method
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // If this is the first instance, set it as the singleton instance
        instance = this;

        // Ensure the instance persists between scene changes
        DontDestroyOnLoad(gameObject);
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
