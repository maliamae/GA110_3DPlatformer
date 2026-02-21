using System.Collections;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [SerializeField] private Transform currentCheckpoint;
    [SerializeField] private GameObject player;
    public GameObject respawnScreen;
    private float screenAlpha;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        screenAlpha = respawnScreen.GetComponentInChildren<CanvasGroup>().alpha;
    }

    private void OnEnable()
    {
        GameManager.OnNewCheckpoint += SetCheckpoint;
        GameManager.OnGameStateChanged += RespawnPlayer;
    }

    private void OnDisable()
    {
        GameManager.OnNewCheckpoint -= SetCheckpoint;
        GameManager.OnGameStateChanged -= RespawnPlayer;
    }

    private void SetCheckpoint(Vector3 newCheckpoint)
    {
        currentCheckpoint.position = newCheckpoint;
    }

    private void RespawnPlayer(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Respawning)
        {
            //player.GetComponent<PlayerMovement>().animator.enabled = true;
            

            StartCoroutine(TransitionRespawn());
        }
    }

    IEnumerator TransitionRespawn()
    {
        respawnScreen.SetActive(true);
        while (respawnScreen.GetComponentInChildren<CanvasGroup>().alpha < 1)
        {
            respawnScreen.GetComponentInChildren<CanvasGroup>().alpha += Time.deltaTime * 1.5f;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        player.transform.position = currentCheckpoint.position;
        while (respawnScreen.GetComponentInChildren<CanvasGroup>().alpha > 0)
        {
            respawnScreen.GetComponentInChildren<CanvasGroup>().alpha -= Time.deltaTime * 1.5f;
            yield return null;
        }
        GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);
    }

}
