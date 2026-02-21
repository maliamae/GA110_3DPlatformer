using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //set respawn point to this tranform
        //destroy this object

        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.SetNewCheckpoint(this.transform.position);
            Destroy(gameObject);
        }
    }
}
