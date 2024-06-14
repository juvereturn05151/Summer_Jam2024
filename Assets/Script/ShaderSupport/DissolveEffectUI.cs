using UnityEngine;
using UnityEngine.UI;

public class DissolveEffectUI : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private float _dissolveSpeed = 0.5f;

    private float _dissolveAmount;
    private bool _isDissolving;

    private void Start()
    {
        _image.material.SetFloat("_DissolveAmount", 1);
        _dissolveAmount = _image.material.GetFloat("_DissolveAmount"); 
    }

    private void Update()
    {
        if (_isDissolving)
        {
            _dissolveAmount = Mathf.Clamp01(_dissolveAmount +_dissolveAmount * Time.deltaTime);
            _image.material.SetFloat("_DissolveAmount", _dissolveAmount);
        }
        else 
        {
            _dissolveAmount = Mathf.Clamp01(_dissolveAmount - ( _dissolveAmount * Time.deltaTime * _dissolveSpeed));
            _image.material.SetFloat("_DissolveAmount", _dissolveAmount);
        }
    }

    public void StartDissolve() 
    {
        _isDissolving = true;
        
    }
}
