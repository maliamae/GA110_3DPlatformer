using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private InputActionAsset playerInputActions; //player's movement action asset

    public static Action OnPlayerDash;
    public static Action<float, bool> OnPlayerMove;
    public static Action OnPlayerJump;

    private void Awake()
    {
        //singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

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

    //sends event to subscribers of OnPlayerDash (SoundManager and PromptText)
    public void RaisePlayerDash()
    {
        OnPlayerDash?.Invoke();
    }

    //sends event to subscribers of OnPlayerMove (SoundManager)
    public void RaisePlayerMove(float moveSpeed, bool isPossible)
    {
        OnPlayerMove?.Invoke(moveSpeed, isPossible);
    }

    //sends event to subscribers of OnPlayerJump (SoundManager)
    public void RaisePlayerJump()
    {
        OnPlayerJump?.Invoke();
    }
}
