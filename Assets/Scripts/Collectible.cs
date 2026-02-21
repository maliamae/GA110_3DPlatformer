using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] int amount = 1; //base amount of rays counted when object is collected
    [SerializeField] CollectibleType type; //type of collectible

    public enum CollectibleType
    {
        Light //only collecting lightrays for now
    }

    private void Start()
    {
        switch(type)
        {
            case CollectibleType.Light:
                GetComponent<MeshRenderer>().material.color = Color.yellow; //changes material color of collectible in accordance with type to display difference from start
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //if the player hits the collectible, collectible amount is added to the curent amount and this object is destroyed
        {
            //CollectibleManager.Instance.Add(type, amount);
            CollectibleEventSystem.RaiseCollectibleCollected(type, amount); //calls function from even system script that triggers event that passes along values to manager and UI manager
            Destroy(gameObject);
        }
    }
}
