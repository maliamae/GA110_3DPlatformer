using UnityEngine;

public class MainMenuControl : MonoBehaviour
{
    //Tied to start button in main menu
    public void StartGame()
    {
        GameManager.Instance.LoadGame();
    }

    //tied to exit button in main menu
    public void ExitGame()
    {
        Debug.Log("GameQuit");
        Application.Quit();
    }

    //tied to replay button in win scene
    public void ReplayGame()
    {
        GameManager.Instance.ReturnToMenu();
    }
}
