using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerInputActions;

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
        

        
        if (state == GameManager.GameState.Playing)
        {
            playerInputActions.Enable();
            Debug.Log("inputs accepted");
        }
        else
        {
            playerInputActions.Disable();
            Debug.Log("inputs disabled");
        }
        
    }
}
