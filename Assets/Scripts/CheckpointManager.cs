using System.Collections;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [SerializeField] private Transform currentCheckpoint; //last checkpoint/respawn point
    [SerializeField] private int savedRays; //amount of rays collected when at last checkpoint
    [SerializeField] private GameObject player; //player game object
    public GameObject respawnScreen; //solid black screen for fade transitions

    private void Awake()
    {
        //singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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

    //sets saved checkpoint position and ray amount to new checkpoint values recorded
    private void SetCheckpoint(Vector3 newCheckpoint, int rays)
    {
        currentCheckpoint.position = newCheckpoint;
        savedRays = rays;
    }

    //Respawn character when in respawning game state
    private void RespawnPlayer(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Respawning)
        {
            GameManager.Instance.ResetSavedRays(Collectible.CollectibleType.Light, savedRays); //updates CollectibleManager and UI with ray amount from last recorded checkpoint
            StartCoroutine(TransitionRespawn()); //move player to last checkpoint
        }
    }

    IEnumerator TransitionRespawn()
    {
        respawnScreen.SetActive(true); //black screen is enabled
        //fade in black screen
        while (respawnScreen.GetComponentInChildren<CanvasGroup>().alpha < 1)
        {
            respawnScreen.GetComponentInChildren<CanvasGroup>().alpha += Time.deltaTime * 1.5f;
            yield return null;
        }
        yield return new WaitForSeconds(1f); //pause
        player.transform.position = currentCheckpoint.position; //move player to last checkpoint
        //fade out black screen
        while (respawnScreen.GetComponentInChildren<CanvasGroup>().alpha > 0)
        {
            respawnScreen.GetComponentInChildren<CanvasGroup>().alpha -= Time.deltaTime * 1.5f;
            yield return null;
        }

        GameManager.Instance.UpdateGameState(GameManager.GameState.Playing); //allow player input again
    }

}
