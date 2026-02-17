using UnityEngine;

public class GameManager : MonoBehaviour
{
    //win/lose condition
    //pause, load next level, saves (menu system)
    public static GameManager Instance;

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
}
