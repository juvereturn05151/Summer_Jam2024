using System.Collections;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Human is the player's main character that has Water as their energy and health
/// The reason why it is singleton because we expected to have only 1 human in the stage
/// </summary>

public class Human : MonoBehaviour
{
    private static Human instance;
    private static string _scriptName = "Human";
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
                    GameObject singletonObject = new GameObject(_scriptName);
                    instance = singletonObject.AddComponent<Human>();
                }

                // Ensure the instance persists between scene changes
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    [SerializeField] 
    private float startWater;

    [SerializeField] 
    private float maxWater;

    [Header("Feedbacks")]
    [SerializeField] 
    private MMF_Player playerHurtFeedback;
    public MMF_Player PlayerHurtFeedback => playerHurtFeedback;

    [SerializeField] 
    private MMF_Player playerFeedback;

    [Header("Particle Prefab")]
    [SerializeField] 
    private GameObject[] bloodParticle;

    [SerializeField] 
    private GameObject deadParticle;

    [SerializeField]
    private ChangeFillImage waterFillImage;
    public ChangeFillImage WaterFillImage => waterFillImage;

    public bool IsHurt { get; private set;}
    public float CurrentWater { get; private set; }

    private const float _lerpSpeed = 2f;
    private const float _moveSpeed = 2f;
    private const float _destroyBloodTime = 5f;
    private const string _horizontalInputString = "Horizontal";
    private const string _verticalInputString = "Vertical";
    private const string _sfx_villageMoveString = "SFX_VillagerMove";
    private const string _sfx_villageDrinkString = "SFX_VillagerDrink";
    private const string _sfx_villageDeadString = "SFX_VillagerDead";
    private const string _sfx_villageHurtString = "SFX_VillagerHurt";
    private const string _sfx_villageHitString = "SFX_VillagerHit";
    private HumanAnimatorController _animator;
    private GameObject _blood;

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

        _animator = GetComponent<HumanAnimatorController>();
    }

    private void Start()
    {
        CurrentWater = startWater;
        waterFillImage = GameplayUIManager.Instance.WaterFillImage;
        GameplayUIManager.Instance.waterSlider.maxValue = maxWater;
        GameplayUIManager.Instance.waterSlider.value = CurrentWater;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.State != GameManager.GameState.StartGame) 
        {
            return;
        }

        HandleMove();
        StartCoroutine(LerpWater());
        CheckPlayerHurtAnim();
        DecreaseWaterAmount(Time.deltaTime);
    }

    public void OnGettingHurt(float damage) 
    {
        IsHurt = true;
        WaterFillImage.HurtSlider();
        PlayerHurtFeedback.PlayFeedbacks();
        SoundManager.Instance.Play(_sfx_villageHurtString);
        SoundManager.Instance.PlayOneShot(_sfx_villageHitString);
        DecreaseWaterAmount(damage);
        WaterFillImage.HurtSlider();
    }

    public void IncreaseWaterAmount(float increaseAmount)
    {
        playerFeedback.PlayFeedbacks();
        CurrentWater += increaseAmount;
        SoundManager.Instance.PlayOneShot(_sfx_villageDrinkString);

        if (CurrentWater >= maxWater) 
        {
            CurrentWater = maxWater;
        }
    }

    public void DecreaseWaterAmount(float increaseAmount)
    {
        if (GameManager.Instance.IsTutorial && !AdvancedTutorialManager.Instance.CurrentTutorial.IsWaterDecreasable())
            return;

        CurrentWater -= increaseAmount;

        if (IsHurt)
        {
            var random = Random.Range(0, 1);
            _blood = Instantiate(bloodParticle[random], transform.position, quaternion.identity, transform);
            Destroy(_blood, _destroyBloodTime);
            IsHurt = false;
        }

        CurrentWater = (CurrentWater - increaseAmount <= 0) ? 0 : CurrentWater - increaseAmount;
        GameplayUIManager.Instance.waterSlider.value = CurrentWater;

        if (GameManager.Instance.IsTutorial) 
        {
            return;
        }

        if (CurrentWater <= 0)
        {
            GameplayUIManager.Instance.OnGameOver(ScoreManager.Scores[GameManager.Instance.CurrentStage] > GameManager.Instance.Phase4Score);

            if (ScoreManager.Scores[GameManager.Instance.CurrentStage] > GameManager.Instance.Phase4Score)
            {
                GameManager.Instance.DestroyOnWinning();
                _animator.StartWinAnimation();
            }
            else 
            {
                SoundManager.Instance.PlayOneShot(_sfx_villageDeadString);
                _animator.StartDeadAnimation();
                var soul = Instantiate(deadParticle, transform.position, quaternion.identity);
            }

            GameManager.Instance.OnEndGame();
        }
    }

    private void HandleMove() 
    {
        float horizontalInput = Input.GetAxis(_horizontalInputString);
        float verticalInput = Input.GetAxis(_verticalInputString);
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        if (horizontalInput != 0f || verticalInput != 0f)
        {
            SoundManager.Instance.PlayWhileOtherSoundIsNotPlaying(_sfx_villageMoveString);
            _animator.StartWalkingAnimation();
        }
        else
        {
            SoundManager.Instance.Stop(_sfx_villageMoveString);
            _animator.StopWalkingAnimation();
        }

        transform.position += moveDirection * _moveSpeed * Time.deltaTime;
    }
    
    private IEnumerator LerpWater()
    {
        GameplayUIManager.Instance.waterSlider.value = Mathf.Lerp(GameplayUIManager.Instance.waterSlider.value, CurrentWater, Time.deltaTime * _lerpSpeed);
        yield return null;
    }

    private void CheckPlayerHurtAnim()
    {
        if (_blood == null)
        {
            _animator.StopHurtAnimation();
        }
        else
        {
            _animator.StopWalkingAnimation();
            _animator.StartHurtAnimation();
        }
    }
}
