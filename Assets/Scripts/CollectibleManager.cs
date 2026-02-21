using UnityEngine;
using System.Collections.Generic;
using static Collectible; //reference collectible script

//subscriber to CollectibleEventSystem
public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }

    private Dictionary<CollectibleType, int> collectibles = new Dictionary<CollectibleType, int>(); //creates dictionary to store different collectible types and their respective value

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

        //Initialize all collectible types to 0
        foreach (CollectibleType type in System.Enum.GetValues(typeof(CollectibleType)))
        {
            collectibles[type] = 0;
        }
    }

    private void OnEnable()
    {
        //subscriber to the collectible event
        CollectibleEventSystem.OnCollectibleCollected += HandleCollectibleCollected; // += adds method defined here to the list of subscribers to the event
        GameManager.OnRespawn += SetCollectibleAmount; //listens for OnRespawn event to be triggered
        
    }

    private void OnDisable()
    {
        //unsubscribe from event
        CollectibleEventSystem.OnCollectibleCollected -= HandleCollectibleCollected;
        GameManager.OnRespawn -= SetCollectibleAmount;

    }

    //event handler method that executes when event is fired
    private void HandleCollectibleCollected(CollectibleType type, int amount)
    {
        if (!collectibles.ContainsKey(type))
            collectibles[type] = 0;
        collectibles[type] += amount;

        CollectibleEventSystem.RaiseCollectiblesUpdated(); //function that triggers UI updates
    }

    //sets amount of rays to an input that has been passed along from CheckpointManager via GameManager events
    private void SetCollectibleAmount(CollectibleType type, int amount)
    {
        if (collectibles.ContainsKey(type))
            collectibles[type] = amount;
        //Debug.Log($"{CollectibleType.Light}: {collectibles[CollectibleType.Light]}");
    }

    //accesses current amount
    public int GetAmount(CollectibleType type)
    {
        if (collectibles.ContainsKey(type))
            return collectibles[type];

        return 0;
    }

}
