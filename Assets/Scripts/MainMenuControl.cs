using UnityEngine;

public class MainMenuControl : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.LoadGame();
    }

    public void ExitGame()
    {
        Debug.Log("GameQuit");
        Application.Quit();
    }

    public void ReplayGame()
    {
        GameManager.Instance.ReturnToMenu();
    }
}
