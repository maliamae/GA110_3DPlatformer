using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //win/lose condition
    //pause, load next level, saves (menu system)
    public static GameManager Instance;
    
    //different possible game states
    public enum GameState 
    {
        StartMenu,
        Playing,
        PlayerDead,
        Respawning,
        Win
    }

    public GameState state;

    public static event Action<GameState> OnGameStateChanged; //triggered whenever the game state changes

    public static event Action<Vector3, int> OnNewCheckpoint; //triggered by SetNewCheckpoint function which is called on trigger of checkpoint bounds

    public static event Action<Collectible.CollectibleType, int> OnRespawn; //triggered by ResetSavedRays function called in CheckpointManager when respawning player

    private void Awake()
    {
        //singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //set initial game state
    private void Start()
    {
        UpdateGameState(GameState.StartMenu);
    }

    //set game state directly
    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.StartMenu:
                break;
            case GameState.Playing:
                break;
            case GameState.PlayerDead:
                Cursor.lockState = CursorLockMode.None;
                break;
            case GameState.Respawning:
                break;
            case GameState.Win:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    //saves last checkpoint position and ray amount data
    public void SetNewCheckpoint(Vector3 checkpoint, int rays)
    {
        OnNewCheckpoint?.Invoke(checkpoint, rays);
    }

    //passes last saved checkpoint's amount data to CollectibleManager
    public void ResetSavedRays(Collectible.CollectibleType type, int rays)
    {
        OnRespawn?.Invoke(type, rays);
    }

}
