using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] int amount = 1;
    [SerializeField] CollectibleType type;

    public enum CollectibleType
    {
        Coin,
        Gem
    }

    private void Start()
    {
        switch(type)
        {
            case CollectibleType.Coin:
                GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;
            case CollectibleType.Gem:
                GetComponent<MeshRenderer>().material.color = Color.magenta;
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //CollectibleManager.Instance.Add(type, amount);
            CollectibleEventSystem.RaiseCollectibleCollected(type, amount);
            Destroy(gameObject);
        }
    }
}
