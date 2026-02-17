using UnityEngine;
using System;

//C# script reperesents messaging not bahvior 
public class CollectibleEventSystem
{
    //Define a static event for when a collectible is collected
    public static event Action<Collectible.CollectibleType, int> OnCollectibleCollected; //declares a static C# event

    //Fired AFTER totals are updated
    public static event Action OnCollectiblesUpdated;

    // Method to invoke the event
    public static void RaiseCollectibleCollected(Collectible.CollectibleType type, int amount)
    {
        OnCollectibleCollected?.Invoke(type, amount);
    }
    public static void RaiseCollectiblesUpdated()
    {
        OnCollectiblesUpdated?.Invoke();
    }
}
