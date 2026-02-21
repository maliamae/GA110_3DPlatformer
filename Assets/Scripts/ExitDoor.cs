using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Win);
        }
    }
}
