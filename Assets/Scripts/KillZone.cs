using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.PlayerDead);
            //GameManager.Instance.ReturnToMenu();
        }
    }
}
