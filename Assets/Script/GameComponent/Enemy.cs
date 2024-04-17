using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AIAgent _aiAgent;

    [Header("Enemy Stats")]
    [SerializeField] private GameObject _stunEffect;
    public GameObject StunEffect => _stunEffect;

    [SerializeField] private GameObject freezeEffect;
    public GameObject FreezeEffect => freezeEffect;

    [SerializeField] private float _health;

    [SerializeField] private float _decreaseAmount;

    [SerializeField] private bool isStunning;

    public float raycastDistance = 1f;

    public bool IsStunning
    {
        get => isStunning;
        set => isStunning = value;
    }

    [Space]
    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private float destroyDropItemTime = 10f;

    [Space]
    [SerializeField] private bool isFreeze;

    public bool IsFreeze
    {
        get => isFreeze;
        set => isFreeze = value;
    }

    [SerializeField] private bool foundHuman;
    [SerializeField] private float scorePoint;

    [SerializeField]
    private GameObject _healthUI;

    private float _maxHealth;

    public LayerMask obstacleLayer;

    public bool FoundHuman
    {
        get => foundHuman;
        set => foundHuman = value;
    }
    
    [Header("Feedbacks")]
    [SerializeField] private GameObject _particleSystem;

    [SerializeField] private MMF_Player feedbacks;

    private void Start()
    {
        _maxHealth = _health;
    }

    private void Update()
    {
        if (TimeManager.Instance._TimePhase == TimePhase.Night)
            Freeze();
        else
        {
            if(isStunning) 
                _aiAgent.ChangeState(new StateSeek(_aiAgent, Human.Instance.gameObject));
        }

        if (_healthUI != null) 
        {
            _healthUI.transform.localScale = new Vector3(_health / _maxHealth, _healthUI.transform.localScale.y, _healthUI.transform.localScale.z);
        }

        Vector2 direction = (Human.Instance.gameObject.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, obstacleLayer);

        if (hit.collider != null)
        {
            // If an obstacle is detected, calculate a new direction to avoid it
            Vector2 avoidDirection = Vector2.Perpendicular(hit.normal).normalized;
            direction += avoidDirection * 2; // Adjust the direction to avoid the obstacle
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Human>() is Human human)
        {
            human.PlayerHurtFeedback.PlayFeedbacks();
            human.DecreaseWaterAmount(_decreaseAmount);
        }
    }

    public void DecreaseHealth(float damage)
    {
        // _health -= Time.deltaTime;
        _health -= damage;
        
        feedbacks.PlayFeedbacks();
        
        Debug.Log("_health" + _health);
        if (_health <= 0) 
        {
            var burnMeltFx = Instantiate(_particleSystem, transform.position + Vector3.up, quaternion.identity);
            _particleSystem.GetComponent<ParticleSystem>().Play(true);
            ScoreManager.score += scorePoint;
            var water = Instantiate(dropItemPrefab, transform.position, quaternion.identity);
            Destroy(burnMeltFx, 2f);
            Destroy(gameObject);
        }
    }

    public void Stun() 
    {
        _aiAgent.ChangeState(new StateStun(_aiAgent, this));
    }

    public void Freeze()
    {
        if(!foundHuman)
            _aiAgent.ChangeState(new StateFreeze(_aiAgent, this));
    }
}
