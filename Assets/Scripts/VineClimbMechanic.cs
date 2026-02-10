using UnityEngine;

public class VineClimbMechanic : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collides with Wall");
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.Climb();
            Debug.Log("player climbing");
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }
}
