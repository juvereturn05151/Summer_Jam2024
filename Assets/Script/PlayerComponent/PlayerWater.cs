using UnityEngine;/*MonoBehaviour*/
using System.Collections; /*IEnumerator*/
using UnityEngine.UI;

public class PlayerWater : MonoBehaviour
{
    [SerializeField]
    private float _startWater;

    [SerializeField]
    private float _maxWater;

    public float CurrentWater { get; private set; }
    public ChangeFillImage WaterFillImage { get; private set; }
    private const string _sfx_villageDrinkString = "SFX_VillagerDrink";
    private const float _lerpSpeed = 2f;
    private GameManager _gameManager;
    private GameplayUIManager _gameplayUIManager;
    private Slider _waterSlider;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameplayUIManager = GameplayUIManager.Instance;
        CurrentWater = _startWater;

        if (_gameplayUIManager != null) 
        {
            _waterSlider = _gameplayUIManager.WaterSlider;
            WaterFillImage = _gameplayUIManager.WaterFillImage;

            if (_waterSlider != null) 
            {
                _waterSlider.maxValue = _maxWater;
                _waterSlider.value = CurrentWater;
            }
        }
    }

    private void Update()
    {
        if (_gameManager.State == GameState.PlayingState)
        {
            StartCoroutine(LerpWater());
            DecreaseWaterAmount(Time.deltaTime * _gameManager.GameTimeScale);
        }
    }

    private IEnumerator LerpWater()
    {
        _waterSlider.value = Mathf.Lerp(GameplayUIManager.Instance.WaterSlider.value, CurrentWater, Time.deltaTime * _lerpSpeed);
        yield return null;
    }

    public void IncreaseWaterAmount(float increaseAmount)
    {
        CurrentWater += increaseAmount;
        SoundManager.Instance.PlayOneShot(_sfx_villageDrinkString);

        if (CurrentWater >= _maxWater)
        {
            CurrentWater = _maxWater;
        }
    }

    public void DecreaseWaterAmount(float increaseAmount)
    {
        if (_gameManager.Mode == GameMode.TutorialMode && !AdvancedTutorialManager.Instance.CurrentTutorial.IsWaterDecreasable())
        {
            return;
        }

        CurrentWater -= increaseAmount;
        CurrentWater = (CurrentWater - increaseAmount <= 0) ? 0 : CurrentWater - increaseAmount;
        _waterSlider.value = CurrentWater;

        if (_gameManager.Mode == GameMode.TutorialMode)
        {
            return;
        }

        if (CurrentWater <= 0)
        {
            GameManager.OnGameEnd(false, _gameManager.Mode);
        }
    }
}
