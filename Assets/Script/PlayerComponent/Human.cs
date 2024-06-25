/* filename Human.cs
 * author   Tonnattan Chankasemporn
 * email:   juvereturn@gmail.com
 * 
 * Brief Description: 
 * Human is the player's main character that has Water as their energy and health
 * The reason why it is singleton because we expected to have only 1 human in the stage (I know this is not a good design)
 * /*/

using System.Collections;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Human : MonoBehaviour, ISetPlayerManager
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
                instance = FindAnyObjectByType<Human>();

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
    private HumanAnimatorController _spiritCharacter;

    [SerializeField]
    private Rigidbody2D _rigidbody2D;

    [SerializeField]
    private PlayerWater _playerWater;

    public ChangeFillImage WaterFillImage { get; private set; }

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
    private Vector3 _moveDirection;
    private HumanAnimatorController _animator;
    private GameObject _blood;
    private PlayerController _playerController;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        _animator = GetComponent<HumanAnimatorController>();
    }

    private void Start()
    {
        CurrentWater = startWater;
        WaterFillImage = GameplayUIManager.Instance.WaterFillImage;
        GameplayUIManager.Instance.WaterSlider.maxValue = maxWater;
        GameplayUIManager.Instance.WaterSlider.value = CurrentWater;
        GameManager.OnGameEnd += OnGameEnd;
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.PlayingState) 
        {
            return;
        }

        CheckPlayerHurtAnim();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State != GameState.PlayingState)
        {
            return;
        }

        HandleMove();
    }

    /*Breif:
     * Receive Value From PlayerController
     */
    public void ReceiveMoveValue(Vector2 movement)
    {
        bool _isWalking = movement.magnitude != 0f;
        _moveDirection = new Vector3(movement.x, movement.y, 0f).normalized;

        if (_isWalking)
        {
            SoundManager.Instance.PlayWhileOtherSoundIsNotPlaying(_sfx_villageMoveString);
            _animator.StartWalkingAnimation();
        }
        else
        {
            SoundManager.Instance.Stop(_sfx_villageMoveString);
            _animator.StopWalkingAnimation();
        }
    }

    public void SetPlayerManager(PlayerController playerManager) 
    {
        _playerController = playerManager;
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
        _playerWater.IncreaseWaterAmount(increaseAmount);
    }

    public void DecreaseWaterAmount(float decreaseAmount)
    {
        if (GameManager.Instance.Mode == GameMode.TutorialMode && !AdvancedTutorialManager.Instance.CurrentTutorial.IsWaterDecreasable()) 
        {
            return;
        } 

        if (IsHurt)
        {
            var random = Random.Range(0, 1);
            _blood = Instantiate(bloodParticle[random], transform.position, quaternion.identity, transform);
            Destroy(_blood, _destroyBloodTime);
            IsHurt = false;
        }

        _playerWater.DecreaseWaterAmount(decreaseAmount);
    }

    private void OnGameEnd(bool isWinning, GameMode mode) 
    {
        GameplayUIManager.Instance.OnGameOver(isWinning, mode);

        if (isWinning)
        {
            OnWinning(isWinning);
        }
        else 
        {
            SoundManager.Instance.PlayOneShot(_sfx_villageDeadString);
            _animator.StartDeadAnimation();
            var soul = Instantiate(deadParticle, transform.position, quaternion.identity);
        }
    }

    public void OnWinning(bool isWin) 
    {
        _animator.StartWinAnimation();
    }

    private void HandleMove() 
    {
        transform.position += _moveDirection * _moveSpeed * GameManager.Instance.GameTimeScale * Time.deltaTime;
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
