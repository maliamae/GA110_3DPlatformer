using UnityEngine;
using System.Collections.Generic;
using static Collectible; //reference collectible script

//subscriber to CollectibleEventSystem
public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }
    //[SerializeField] int coinsTotal;
    //[SerializeField] int gemsTotal;
    //public int totalCollected;

    private Dictionary<CollectibleType, int> collectibles =
        new Dictionary<CollectibleType, int>();

    private void Awake()
    {
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
    }

    private void OnDisable()
    {
        //unsubscribe from event
        CollectibleEventSystem.OnCollectibleCollected -= HandleCollectibleCollected;
    }

    //event handler method that executes when event is fired
    private void HandleCollectibleCollected(CollectibleType type, int amount)
    {
        if (!collectibles.ContainsKey(type))
            collectibles[type] = 0;
        collectibles[type] += amount;

        Debug.Log($"{type}: {collectibles[type]}");

        CollectibleEventSystem.RaiseCollectiblesUpdated();
    }

    /*
    public void Add(CollectibleType type, int amount)
    {
        if (!collectibles.ContainsKey(type))
            collectibles[type] = 0;

        collectibles[type] += amount;

        Debug.Log($"{type}: {collectibles[type]}");

        
    }
    */

    public int GetAmount(CollectibleType type)
    {
        if (collectibles.ContainsKey(type))
            return collectibles[type];

        return 0;
    }

}
