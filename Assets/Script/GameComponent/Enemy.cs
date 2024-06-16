using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private AIAgent _aiAgent;

    [Header("Enemy Stats")]
    [SerializeField] 
    private GameObject _stunEffect;
    public GameObject StunEffect => _stunEffect;

    [SerializeField] 
    private GameObject freezeEffect;

    [SerializeField]
    private float _health;

    [SerializeField] 
    private float _decreaseAmount;

    [SerializeField]
    private bool _unstunnable;

    [SerializeField] 
    private bool isStunning;
    public bool IsStunning => isStunning;

    [SerializeField]
    private float raycastDistance = 1f;

    [Space]
    [SerializeField]
    private GameObject dropItemPrefab;

    [Space]
    [SerializeField]
    private bool isFreeze;

    [SerializeField] 
    private int scorePoint;

    [SerializeField] 
    private GameObject _healthUI;

    [SerializeField] 
    private GameObject innerHealthUI;

    [SerializeField]
    private LayerMask obstacleLayer;

    [Header("Feedbacks")]
    [SerializeField] 
    private MMF_Player feedbacks;

    [Header("Particle & FX")]
    [SerializeField] 
    private GameObject smokeFX;

    [SerializeField] 
    private GameObject damageFX;

    [SerializeField]
    private GameObject lightFX;

    [SerializeField]
    private Barrier _barrier;
    public Barrier Barrier => _barrier;

    [SerializeField]
    private float _barrierTimer;

    private const float _meltDestroyTime = 0.5f;
    private const float _damageFxDestroyTime = 1f;
    private const float _damageFxTimer = 0.25f;
    private const float _obstacleAvoidanceJitter = 2f;
    private const string _sfx_monsterSpawn = "SFX_MonsterSpawn";
    private const string _sfx_enemyHurt = "SFX_EnemyHurt";
    private const string _sfx_isDead = "isDead";
    private const string _sfx_monsterDead = "SFX_MonsterDead";
    private float _maxHealth;
    private float _timer;
    private float _currentBarrierCooldownTime;
    private float _currentHealthHorizontalScale;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _maxHealth = _health;
        _currentHealthHorizontalScale = _health / _maxHealth;
        innerHealthUI.transform.localScale = new Vector3(_currentHealthHorizontalScale, 0.5f, _healthUI.transform.localScale.z);
        SoundManager.Instance.PlayOneShot(_sfx_monsterSpawn);
    }

    private void Update()
    {
        if(GameManager.Instance.State != GameState.PlayingState)
        {
            return;
        }

        Seek();
        UpdateHealthUI();
        ObstacleAvoid();
        HandleBarrier();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Human>() is Human human)
        {
            human.OnGettingHurt(_decreaseAmount); 
            var water = Instantiate(GameplayUIManager.Instance.WaterSplashFX, GameplayUIManager.Instance.waterSplashParent.transform.position, quaternion.identity, GameplayUIManager.Instance.waterSplashParent.transform);
            Destroy(water, 1f);
        }
    }

    private void HandleBarrier() 
    {
        if (_barrier != null) 
        {
            if (!_barrier.isActiveAndEnabled)
            {
                _currentBarrierCooldownTime -= Time.deltaTime;

                if (_currentBarrierCooldownTime <= 0)
                {
                    _currentBarrierCooldownTime = _barrierTimer;
                    _barrier.SetBarrierActive(true);
                }
            }
            else 
            {
                _barrier.SetBarrierActive(true);
            }
        }
    }

    public void SetIsStunning(bool stunning) 
    {
        if (_unstunnable) 
        {
            isStunning = false;
            return;
        }

        isStunning = stunning;
    }

    public void DecreaseHealth(float damage)
    {
        if (_health <= 0)
            return;

        if (Barrier !=null && Barrier.isActiveAndEnabled)
            return;

        _health -= damage;
        _timer += Time.deltaTime;

        if (_timer > _damageFxTimer)
        {
            SoundManager.Instance.Play(_sfx_enemyHurt);
            var dmgFX = Instantiate(damageFX, transform.position, quaternion.identity, transform);
            Destroy(dmgFX, _damageFxDestroyTime);
            _timer = 0;
        }

        //feedbacks.PlayFeedbacks();
        
        if (_health <= 0) 
        {
            var burnMeltFx = Instantiate(smokeFX, transform.position + Vector3.up, quaternion.identity);
            GameplayUIManager.Instance.IncreaseScore(scorePoint);
            
            Destroy(burnMeltFx, _meltDestroyTime);
            _animator.SetBool(_sfx_isDead, true);
        }
    }

    public void Stun()
    {
        _aiAgent.ChangeState(new StateStun(_aiAgent, this));
    }

    public void OnDead()
    {
        SoundManager.Instance.PlayOneShot(_sfx_monsterDead);
        var water = Instantiate(dropItemPrefab, transform.position, quaternion.identity);
        var waterSplashFX = Instantiate(lightFX, transform.position, quaternion.identity, water.transform);
        Destroy(waterSplashFX, 2f);
        Destroy(gameObject);
    }

    private void Seek() 
    {
        if (!isStunning)
        {
            _aiAgent.ChangeState(new StateSeek(_aiAgent, Human.Instance.gameObject));
        }
    }

    private void UpdateHealthUI() 
    {
        if (_healthUI != null)
        {
            float healthPercentage = _health / _maxHealth;
            _healthUI.transform.localScale = new Vector3(healthPercentage, 0.5f, _healthUI.transform.localScale.z);
            _healthUI.transform.localPosition = new Vector3((_currentHealthHorizontalScale * (1 - healthPercentage)) / 2, innerHealthUI.transform.localPosition.y, _healthUI.transform.localPosition.z);
        }
    }

    private void ObstacleAvoid() 
    {
        Vector2 direction = (Human.Instance.gameObject.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, obstacleLayer);

        if (hit.collider != null)
        {
            // If an obstacle is detected, calculate a new direction to avoid it
            Vector2 avoidDirection = Vector2.Perpendicular(hit.normal).normalized;
            direction += avoidDirection * _obstacleAvoidanceJitter; // Adjust the direction to avoid the obstacle
        }
    }
}
