using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //win/lose condition
    //pause, load next level, saves (menu system)
    public static GameManager Instance;
    

    public enum GameState
    {
        StartMenu,
        Playing,
        PlayerDead,
        Respawning,
        Win
    }

    public GameState state;

    public static event Action<GameState> OnGameStateChanged;

    public static event Action<Vector3> OnNewCheckpoint;

    private void Awake()
    {
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

    private void Start()
    {
        UpdateGameState(GameState.StartMenu);
    }

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

    public void SetNewCheckpoint(Vector3 checkpoint)
    {
        OnNewCheckpoint?.Invoke(checkpoint);
    }

}
