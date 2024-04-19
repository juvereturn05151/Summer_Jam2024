using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

        animator = GetComponent<HumanAnimatorController>();
    }

    [SerializeField] private HumanAIAgent _humanAIAgent;
    public HumanAIAgent HumanAIAgent => _humanAIAgent;

    private bool isHurt;

    public bool IsHurt
    {
        get => isHurt;
        set => isHurt = value;
    }

    [Header("Feedbacks")] 
    [SerializeField] private MMF_Player playerHurtFeedback;
    public MMF_Player PlayerHurtFeedback
    {
        get => playerHurtFeedback;
        set => playerHurtFeedback = value;
    }
    [SerializeField] private MMF_Player playerFeedback;
    
    [Header("Particle Prefab")]
    [SerializeField] private GameObject[] bloodParticle;
    [SerializeField] private GameObject deadParticle;
    
    private HumanAnimatorController animator;
    private GameObject blood;

    [SerializeField] private ChangeFillImage waterFillImage;

    public ChangeFillImage WaterFillImage
    {
        get => waterFillImage;
        set => waterFillImage = value;
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
        if (GameManager.Instance.State == GameManager.GameState.EndGame)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        if(horizontalInput != 0f || verticalInput != 0f)
        {
            SoundManager.Instance.PlayWhileOtherSoundIsNotPlaying("SFX_VillagerMove");
            animator.StartWalkingAnimation();
        }
        else
        {
            SoundManager.Instance.Stop("SFX_VillagerMove");
            animator.StopWalkingAnimation();
        }

        // Debug.Log("moveDirection: " + moveDirection);
        transform.position += moveDirection * 2f * Time.deltaTime;

        StartCoroutine(LerpWater());
        
        CheckPlayerHurtAnim();
        //DecreaseWaterAmount(Time.deltaTime);
        DecreaseWaterAmount(Time.deltaTime);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.GetComponent<Enemy>() is Enemy enemy)
        // {
        //     DecreaseWaterAmount(30);
        // }
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

        SoundManager.Instance.PlayOneShot("SFX_VillagerDrink");

        if (amountWater >= maxWater)
            amountWater = maxWater;
    }

    public void DecreaseWaterAmount(float increaseAmount)
    {
        // GameplayUIManager.Instance.waterSlider.value = amountWater;
        amountWater -= increaseAmount;

        if (isHurt)
        {
            var random = Random.Range(0, 1);
            blood = Instantiate(bloodParticle[random], transform.position, quaternion.identity, transform);
            Destroy(blood, 5f);
            isHurt = false;
        }

        amountWater = (amountWater - increaseAmount <= 0) ? 0 : amountWater - increaseAmount;
        
        GameplayUIManager.Instance.waterSlider.value = amountWater;

        if (amountWater <= 0)
        {
            SoundManager.Instance.PlayOneShot("SFX_VillagerDead");
            animator.StartDeadAnimation();
            GameManager.Instance.OnEndGame();
            GameplayUIManager.Instance.gameOverUI.SetActive(true);
            if (GameManager.Instance.State == GameManager.GameState.EndGame)
            {
                var soul = Instantiate(deadParticle, transform.position, quaternion.identity);
            }
        }
            
    }

    public void SetMovingAnimation(bool isMoving)
    {
        if(isMoving)
        {
            animator.StartWalkingAnimation();
        }
        else
        {
            animator.StopWalkingAnimation();
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

    void CheckPlayerHurtAnim()
    {
        if (blood == null)
        {
            animator.StopHurtAnimation();
        }
        else
        {
            animator.StopWalkingAnimation();
            animator.StartHurtAnimation();
        }
    }
}
