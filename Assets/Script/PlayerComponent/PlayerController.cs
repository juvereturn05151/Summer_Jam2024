/* filename PlayerManager.cs
 * author   Tonnattan Chankasemporn
 * email:   juvereturn@gmail.com
 * 
 * Brief Description: 
 * Manages Player Related Components Including Human And Sunlight Characters
 * /*/

using UnityEngine; /*MonoBehaviour*/
using UnityEngine.InputSystem; /*InputAction.CallbackContext*/

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Human _human;
    public Human Human => _human;

    [SerializeField]
    private PlayerAttack _playerAttack;

    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        if (_human != null)
        {
            _human.SetPlayerManager(this);
        }

        if (_playerAttack) 
        {
            _playerAttack.SetPlayerManager(this);
        }
    }

    public void MoveHuman(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();

        if (_human != null) 
        {
            _human.ReceiveMoveValue(movement);
        }
    }

    public void Attack(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            _playerAttack.OnHoldAttack();
        }
        else if (context.started) 
        {
            _playerAttack.OnTapAttack();
        }
        else if (context.canceled)
        {
            _playerAttack.OnReleaseAttack();
        }
    }
}
