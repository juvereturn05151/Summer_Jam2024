using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    private static Human instance;

    // Public property to access the singleton instance
    public static Human Instance
    {
        get
        {
            // If the instance doesn't exist, try to find it in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<Human>();

                // If it still doesn't exist, create a new GameObject with the SingletonExample component
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("Human");
                    instance = singletonObject.AddComponent<Human>();
                }

                // Ensure the instance persists between scene changes
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    #region -Declared Variables-

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
    }

    [SerializeField] private HumanAIAgent _humanAIAgent;
    public HumanAIAgent HumanAIAgent => _humanAIAgent;

    [SerializeField] private float health;

    [Header("Feedbacks")] 
    [SerializeField] private MMF_Player playerHurtFeedback;
    [SerializeField] private MMF_Player playerFeedback;
    
    public MMF_Player PlayerHurtFeedback
    {
        get => playerHurtFeedback;
        set => playerHurtFeedback = value;
    }

    private void Start()
    {
        amountWater = startWater;
        GameplayUIManager.Instance.waterSlider.maxValue = maxWater;
        GameplayUIManager.Instance.waterSlider.value = amountWater;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        Debug.Log("moveDirection: " + moveDirection);
        transform.position += moveDirection * 2* Time.deltaTime;

        StartCoroutine(LerpWater());
        //DecreaseWaterAmount(Time.deltaTime);
    }

    public void DecreaseHealth(float damage)
    {
        health -= damage;

        print($"health : {health}");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Enemy>() is Enemy enemy)
        {
            DecreaseWaterAmount(30);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Moonlight>())
        {
            _humanAIAgent.ChangeState(new StateRetreat(_humanAIAgent));
        }
    }

    public void IncreaseWaterAmount(float increaseAmount)
    {
        // GameplayUIManager.Instance.waterSlider.value = amountWater;
        playerFeedback.PlayFeedbacks();
        amountWater += increaseAmount;

        if (amountWater >= maxWater)
            amountWater = maxWater;
    }

    public void DecreaseWaterAmount(float increaseAmount)
    {
        // GameplayUIManager.Instance.waterSlider.value = amountWater;
        amountWater -= increaseAmount;

        if (amountWater <= 0) 
        {
            amountWater = 0;
            GameplayUIManager.Instance.gameOverUI.SetActive(true);
        }
            
    }

    public void MoveTo(Vector3 pos) 
    {
        _humanAIAgent.Seek(pos);
    }
    
    IEnumerator LerpWater()
    {
        GameplayUIManager.Instance.waterSlider.value = Mathf.Lerp(GameplayUIManager.Instance.waterSlider.value, amountWater, Time.deltaTime * lerpSpeed);
        yield return null;
    }
}
