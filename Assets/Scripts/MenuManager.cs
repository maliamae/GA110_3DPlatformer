using System;
using UnityEngine;
using UnityEngine.Android;

public class MenuManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject collectibleScreen;
    public GameObject deathScreen;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleMenuChange;
    }

   

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleMenuChange;
    }

    private void HandleMenuChange(GameManager.GameState state)
    {
        startScreen.SetActive(state == GameManager.GameState.StartMenu);
        collectibleScreen.SetActive(state == GameManager.GameState.Playing);
        deathScreen.SetActive(state == GameManager.GameState.PlayerDead);
    }

    public void OnClickPlay()
    {
        //startScreen.SetActive(false);
        GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);
    }

    public void OnRespawnClick()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Respawning);
    }

}
