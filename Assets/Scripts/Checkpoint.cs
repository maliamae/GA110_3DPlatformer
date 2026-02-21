using UnityEngine;
using static Collectible;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //passes values of checkpoint position and player's current ray amount to CheckpointManager
        if (other.gameObject.tag == "Player")
        {
            int rays = CollectibleManager.Instance.GetAmount(CollectibleType.Light);
            GameManager.Instance.SetNewCheckpoint(this.transform.position, rays);
            Destroy(gameObject);
        }
    }
}
