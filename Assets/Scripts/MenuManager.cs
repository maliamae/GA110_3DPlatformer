using System;
using UnityEngine;
using UnityEngine.Android;

public class MenuManager : MonoBehaviour
{
    public GameObject startScreen; //canvas with starting UI
    public GameObject collectibleScreen; //canvas with collectible UI
    public GameObject deathScreen; //canvas with death UI
    public GameObject winScreen; //canvas with win UI

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleMenuChange; //listens for game state changes
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleMenuChange;
    }

    //changes of active canvases depending on game state
    private void HandleMenuChange(GameManager.GameState state)
    {
        startScreen.SetActive(state == GameManager.GameState.StartMenu);
        collectibleScreen.SetActive(state == GameManager.GameState.Playing);
        deathScreen.SetActive(state == GameManager.GameState.PlayerDead);
        winScreen.SetActive(state == GameManager.GameState.Win);
    }

    //start screen "Play" button
    public void OnClickPlay()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);
    }

    //death screen "Respawn" button
    public void OnRespawnClick()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Respawning);
    }

}
