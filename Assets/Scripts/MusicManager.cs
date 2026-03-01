using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource menuMusicSource;
    [SerializeField] private AudioSource gameplayMusicSource;
    [SerializeField] private AudioSource winMusicSource;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string resetSceneName = "ResetScene";
    [SerializeField] private string gameplaySceneName = "Scene1";
    [SerializeField] private string winSceneName = "WinScene";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        menuMusicSource.loop = true;
        gameplayMusicSource.loop = true;
        winMusicSource.loop = true;
        menuMusicSource.Play();
        gameplayMusicSource.Pause();

        SceneManager.activeSceneChanged += OnSceneChanged; //listens for when the scene is changed to play the corresponding music
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        //ignore resetscene
        if (newScene.name == resetSceneName)
        {
            return;
        }

        //switch music based on scene name
        if (newScene.name == mainMenuSceneName)
        {
            gameplayMusicSource.Stop();
            winMusicSource.Stop();
            menuMusicSource.Play();
        }
        else if (newScene.name == gameplaySceneName)
        {
            menuMusicSource.Stop();
            winMusicSource.Stop();
            gameplayMusicSource.Play();
        }
        else if (newScene.name == winSceneName)
        {
            menuMusicSource.Stop();
            gameplayMusicSource.Stop();
            winMusicSource.Play();
        }
    }
}
