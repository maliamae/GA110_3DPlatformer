using UnityEngine;
using TMPro;
using static Collectible;

public class CollectibleUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text lightText;
    [SerializeField] private int rays; //serialized for debugging; display amount of rays collected
    //[SerializeField] private TMP_Text gemsText;

    private void OnEnable()
    { 
        UpdateUI(); //called on enable to keep most up to date amount of rays displayed since this screen is disabled when in certain game states
        CollectibleEventSystem.OnCollectiblesUpdated += UpdateUI; 
    }
    private void OnDisable()
    { 
        CollectibleEventSystem.OnCollectiblesUpdated -= UpdateUI; 
    }

    private void UpdateUI() //updates the displayed amount of rays the player has collected
    {
        if (CollectibleManager.Instance == null)
            return;
        rays = CollectibleManager.Instance.GetAmount(CollectibleType.Light); //gets amount of rays 

        //Debug.Log("Get: " + CollectibleManager.Instance.GetAmount(CollectibleType.Light));
        lightText.text = $"Lightrays: {rays}"; //updates text
        
    }
}
