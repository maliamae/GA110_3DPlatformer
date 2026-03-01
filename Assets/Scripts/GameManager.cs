using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //win/lose condition
    //pause, load next level, saves (menu system)
    public static GameManager Instance;

    public string nextSceneName;

    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string resetSceneName = "ResetScene";
    [SerializeField] private string gameplaySceneName = "Scene1";
    [SerializeField] private string winSceneName = "WinScene";

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
                Cursor.lockState = CursorLockMode.None;
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

    //loads game scene
    public void LoadGame()
    {
        SceneManager.LoadScene(resetSceneName, LoadSceneMode.Additive);
        UpdateGameState(GameState.Playing);
        nextSceneName = gameplaySceneName;
    }

    //loads main menus
    public void ReturnToMenu()
    {
        //reset totals before leaving gameplay
        if (CollectibleManager.Instance != null)
        {
            CollectibleManager.Instance.ResetCollectibles();
        }

        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(resetSceneName, LoadSceneMode.Additive);
        
        nextSceneName = mainMenuSceneName;
    }

    public void LoadWinScreen()
    {
        SceneManager.LoadScene(resetSceneName, LoadSceneMode.Additive);
        UpdateGameState(GameState.Win);
        nextSceneName = winSceneName;
    }

}
