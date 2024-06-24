using UnityEngine;

public class InputSystem : MonoBehaviour
{
    PlayerInputSystem playerInput;

    Player player;

    private void OnEnable()
    {
        player = GetComponent<Player>();

        if (playerInput == null)
        {
            playerInput = new PlayerInputSystem();

            playerInput.PlayerInput.Move.performed += i => player.moveInput = i.ReadValue<Vector2>();

        }

        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

}
