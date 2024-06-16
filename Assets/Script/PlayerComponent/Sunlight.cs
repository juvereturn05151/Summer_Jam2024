using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using Unity.Mathematics;

public class Sunlight : MonoBehaviour, ISetPlayerManager
{
    [SerializeField]
    private float _waterDecreaseSpeed = 1.5f;
    
    [Header("Sunlight (Sprite Renderer)")]
    [SerializeField] 
    private SpriteRenderer _sunlightImage;

    [SerializeField] 
    private float _startingAlpha = 0.25f;

    [Header("Sunlight (Light)")]
    [SerializeField] 
    private Light2D light;

    [SerializeField] 
    private float sunlightRadius = 0.5f;

    [SerializeField] 
    private float normalSunlightIntensity = 0.1f;
    [SerializeField]
    private float sunlightFlashIntensity = 0.5f;

    [SerializeField] 
    private float timeTargetToDamage;

    [Header("Sunlight Damage")]
    [SerializeField] 
    private float sunlightDamageToEnemy = 10f;

    [SerializeField]
    private GameObject hole;

    [SerializeField]
    private float createHoldTime;

    [SerializeField] 
    private CircleCollider2D circleCollider2D;

    [Header("Particle FX")]
    [SerializeField] 
    private GameObject lightParticle;

    [SerializeField] 
    private GameObject meltSnowFX;

    [SerializeField] 
    private GameObject fireMeltFX;

    [Space]
    public bool ActivateSunlight;

    private const float _alphaIncreaseSpeed = 2.0f;
    private const float _pointLightIncrementRadiusJitter = 0.75f;
    private const float _originalPointOuterRadiusSize = 1f;
    private const float _originalPointInnerRadiusSize = 0.75f;
    private const float _originalCircleColliderSize = 1f;
    private const float _fireFxDestroyTime = 0.7f;
    private const float _lightFxDestroyTime = 2f;
    private const string _sfx_burn = "SFX_Burn";
    private const string _sfx_pond = "PondSFX";
    private const string _sfx_expand = "SFX_Expand";
    private readonly Color _normal_color = new Color(1.0f, 0.64f, 0.0f, 1);
    private readonly Color _attacking_color = new Color(1.0f, 0.20f, 0.0f, 1);
    private float _maximumAlpha = 1.0f;
    private float _currentAlpha;
    private float _timer;
    private CircleCollider2D _col;
    private List<Enemy> _sightedEnemies = new List<Enemy>();
    private List<Pond> _sightedPonds = new List<Pond>();
    private float _currentCreatePondTime;
    private GameObject _lightFX_Temp;
    private bool _isLighting;
    private PlayerManager _playerManager;

    private void Start()
    {
        SetAlpha(_startingAlpha);

        light = GetComponentInChildren<Light2D>();
        light.intensity = normalSunlightIntensity;

        _col = GetComponent<CircleCollider2D>();
        _col.radius = sunlightRadius;
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.PlayingState || GameManager.Instance.State == GameState.PreparingState) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleHitEnemyOnce();
            }

            if (Input.GetMouseButtonUp(0))
            {
                SoundManager.Instance.Stop(_sfx_burn);
            }

            if (Input.GetMouseButton(0))
            {
                HandlePreparingSunlighting();

                if (IsReadyToSunlighting())
                {
                    StartSunlight();
                }
            }
            else
            {
                StopSunlight();
            }

            if (GameManager.Instance.State == GameState.PreparingState) 
            {
                if (ActivateSunlight)
                {
                    HandleSunlightOnPreparePhase();
                    GameplayUIManager.Instance.PrepareStateManager.OnPlayerSpawn();
                }
            }

            if (GameManager.Instance.State == GameState.PlayingState) 
            {
                if (Input.GetMouseButton(0))
                {
                    if (IsReadyToSunlighting())
                    {
                        if (Human.Instance.CurrentWater > 0)
                        {
                            StartSunlightingAttack();
                        }
                    }
                }
                else
                {
                    StopSunlightAttack();
                }

                if (ActivateSunlight)
                {
                    HandleOnSunlighting();
                }
            }

            HandleSunlightColor();
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() is Enemy enemy)
        {
            if (!_sightedEnemies.Contains(enemy))
            {
                _sightedEnemies.Add(enemy);

                if (ActivateSunlight && _sightedEnemies.Count > 1)
                {
                    SoundManager.Instance.PlayOneShot(_sfx_expand);
                }
            }
        }

        if (collision.gameObject.GetComponent<Pond>() is Pond pond)
        {
            if (!_sightedPonds.Contains(pond))
            {
                _sightedPonds.Add(pond);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() is Enemy enemy)
        {
            if (_sightedEnemies.Contains(enemy))
            {
                _sightedEnemies.Remove(enemy);
            }
        }

        if (collision.gameObject.GetComponent<Pond>() is Pond pond)
        {
            if (_sightedPonds.Contains(pond))
            {
                _sightedPonds.Remove(pond);
            }
        }
    }

    public void SetPlayerManager(PlayerManager playerManager) 
    {
        _playerManager = playerManager;
    }

    private void HandleHitEnemyOnce() 
    {
        SoundManager.Instance.Play(_sfx_burn);

        for (int i = 0; i < _sightedEnemies.Count; i++)
        {
            if (_sightedEnemies[i] != null)
            {
                _sightedEnemies[i].Stun();
                _sightedEnemies[i].DecreaseHealth(sunlightDamageToEnemy * Time.deltaTime);
                _currentCreatePondTime = 0;
            }
        }
    }

    private void HandlePreparingSunlighting() 
    {
        IncreasingAlpha();
        IncreasingLightIntensity();
        HandleLightCircleSize();
    }

    private void IncreasingAlpha() 
    {
        _currentAlpha += Time.deltaTime * _alphaIncreaseSpeed;
        SetAlpha(_currentAlpha);
    }

    private void IncreasingLightIntensity() 
    {
        if (light.intensity < sunlightFlashIntensity)
        {
            light.intensity += Time.deltaTime;
        }
    }

    private void HandleLightCircleSize() 
    {
        if (_sightedEnemies.Count > 1)
        {
            ExpandRadiusWithEnemyCount();
        }
        else
        {
            ResetSunlightRadius();
        }
    }

    private void ExpandRadiusWithEnemyCount() 
    {
        light.pointLightOuterRadius = _sightedEnemies.Count;
        light.pointLightInnerRadius = _sightedEnemies.Count * _pointLightIncrementRadiusJitter;
        circleCollider2D.radius = _sightedEnemies.Count;
    }

    private void ResetSunlightRadius() 
    {
        light.pointLightOuterRadius = _originalPointOuterRadiusSize;
        light.pointLightInnerRadius = _originalPointInnerRadiusSize;
        circleCollider2D.radius = _originalCircleColliderSize;
    }

    private bool IsReadyToSunlighting() 
    {
        return _currentAlpha >= _maximumAlpha || light.intensity >= sunlightFlashIntensity;
    }

    private void StartSunlight()
    {
        ActivateSunlight = true;
    }

    private void StartSunlightingAttack() 
    {
        Human.Instance.CallSpiritPower();
        Human.Instance.DecreaseWaterAmount(Time.deltaTime * _waterDecreaseSpeed);
    }

    private void HandleOnSunlighting() 
    {
        HandleVfxOnSunlighting();
        HandleSightedEnemies();
        HandleSightedPond();
        HandleCreatingBigPond();
    }

    private void HandleSunlightOnPreparePhase() 
    {
        if (GameManager.Instance.State == GameState.PreparingState) 
        {
            _timer += Time.deltaTime;

            if (_timer >= 1f)
            {
                _timer = 0;
                //Spawn Player Here
                if (_playerManager.Human.gameObject.activeInHierarchy) 
                {
                    return;
                }

                _playerManager.Human.gameObject.SetActive(true);
                _playerManager.Human.transform.position = transform.position;
            }
        }
    }

    private void HandleVfxOnSunlighting() 
    {
        _timer += Time.deltaTime;

        if (_timer >= 1f)
        {
            var snowMelt = Instantiate(meltSnowFX, transform.position + Vector3.up, quaternion.identity);
            Destroy(snowMelt, 1f);
            _timer = 0;
        }

        if (_lightFX_Temp == null)
        {
            if (!_isLighting)
            {
                _isLighting = true;
                _lightFX_Temp = Instantiate(lightParticle, transform.position, quaternion.identity, transform);
            }
        }
    }

    private void HandleSightedEnemies() 
    {
        for (int i = 0; i < _sightedEnemies.Count; i++)
        {
            if (_sightedEnemies[i] != null)
            {
                _sightedEnemies[i].Stun();
                _sightedEnemies[i].DecreaseHealth(sunlightDamageToEnemy * Time.deltaTime);
                _currentCreatePondTime = 0;
            }
        }
    }

    private void HandleSightedPond() 
    {
        for (int i = 0; i < _sightedPonds.Count; i++)
        {
            if (_sightedPonds[i] != null)
            {
                _sightedPonds[i].Evaporating();
                _currentCreatePondTime = 0;
            }
        }
    }

    private void HandleCreatingBigPond() 
    {
        _currentCreatePondTime += Time.deltaTime;

        if (_currentCreatePondTime >= createHoldTime)
        {
            SoundManager.Instance.PlayOneShot(_sfx_pond);

            var fireFX = Instantiate(fireMeltFX, transform.position, quaternion.identity);
            Destroy(fireFX, _fireFxDestroyTime);

            var holeClone = Instantiate(hole, transform.position, transform.rotation);
            Pond pond = holeClone.GetComponent<Pond>();

            if (!_sightedPonds.Contains(pond))
            {
                _sightedPonds.Add(pond);
            }

            _currentCreatePondTime = 0;

            var lightFX = Instantiate(lightParticle, holeClone.transform.position, quaternion.identity);
            Destroy(lightFX, _lightFxDestroyTime);
        }
    }

    private void StopSunlight() 
    {
        _currentAlpha = _startingAlpha;
        SetAlpha(_startingAlpha);

        light.intensity = normalSunlightIntensity;
        ResetSunlightRadius();
    }

    private void StopSunlightAttack() 
    {
        ActivateSunlight = false;

        if (GameManager.Instance.State == GameState.PlayingState) 
        {
            Human.Instance.UnCallSpiritPower();
        }
    }

    private void HandleSunlightColor() 
    {
        if (_sightedEnemies.Count == 0)
        {
            light.color = _normal_color;
        }
        else
        {
            light.color = _attacking_color;
        }
    }

    private void SetAlpha(float newAlpha) 
    {
        var tempColor = _sunlightImage.color;
        tempColor.a = newAlpha;
        _sunlightImage.color = tempColor;
    }

    private void AdjustRadius(float radius)
    {
        _col.radius = radius;
    }
}
