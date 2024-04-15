using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    #region -Declared Variables-

    [Header("Water Bar")] 
    [SerializeField] private Slider waterSlider;

    private bool isThirsty;
    private float velocity = 0f;
    private float lerpSpeed = 2f;
    
    [SerializeField] private float startWater;
    [SerializeField] private float maxWater;
    [SerializeField] private float amountWater;
    public float AmountWater
    {
        get => amountWater;
        set => amountWater = value;
    }

    [Header("Human Spawn")]
    [SerializeField] private GameObject humanPrefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject humanInstantiate;
    
    #endregion

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

    private void Start()
    {
        amountWater = startWater;
        waterSlider.maxValue = maxWater;
        waterSlider.value = amountWater;
    }

    private void Update()
    {
        HumanMidnightSpawn();
        if (isThirsty)
        {
            waterSlider.value = Mathf.SmoothDamp(waterSlider.value, amountWater, ref velocity, Time.deltaTime * lerpSpeed);
        }
    }

    void HumanMidnightSpawn()
    {
        if (TimeManager.Instance._TimePhase == TimePhase.Night && humanInstantiate == null)
            humanInstantiate = Instantiate(humanPrefab, spawnPoint.position, quaternion.identity);
        if (TimeManager.Instance._TimePhase == TimePhase.Morning)
            Destroy(humanInstantiate);
    }

    public void IncreaseWaterAmount(float increaseAmount)
    {
        isThirsty = true;
        waterSlider.value = amountWater;
        amountWater += increaseAmount;

        if (amountWater >= maxWater)
            amountWater = maxWater;
    }

    public void DecreaseWaterAmount(float increaseAmount)
    {
        isThirsty = true;
        waterSlider.value = amountWater;
        amountWater -= increaseAmount;

        if (amountWater <= 0)
            amountWater = 0;
    }
}
