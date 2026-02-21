using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerInputActions; //player's movement action asset

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleAcceptInputs;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleAcceptInputs;
    }

    private void HandleAcceptInputs(GameManager.GameState state)
    {
        //disable player inputs being accepted while in anything other than play mode
        if (state == GameManager.GameState.Playing)
        {
            playerInputActions.Enable();
        }
        else
        {
            playerInputActions.Disable();
        }
        
    }
}
