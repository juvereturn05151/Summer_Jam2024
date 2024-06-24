using UnityEngine;

public class InputSystem : MonoBehaviour
{
    PlayerInputSystem playerInput;

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInputSystem();

        }

        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

}
