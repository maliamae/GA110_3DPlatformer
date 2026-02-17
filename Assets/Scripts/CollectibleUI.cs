using UnityEngine;
using TMPro;
using static Collectible;

public class CollectibleUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text gemsText;

    private void OnEnable()
    { 
        CollectibleEventSystem.OnCollectiblesUpdated += UpdateUI; 
    }
    private void OnDisable()
    { 
        CollectibleEventSystem.OnCollectiblesUpdated -= UpdateUI; 
    }

    private void Start()
    {
        UpdateUI(); // Initial refresh}
    }

    private void UpdateUI()
    {
        if (CollectibleManager.Instance == null)
            return;
        int coins = CollectibleManager.Instance.GetAmount(CollectibleType.Coin);
        int gems = CollectibleManager.Instance.GetAmount(CollectibleType.Gem);
        coinsText.text = $"Coins: {coins}";
        gemsText.text = $"Gems: {gems}";
    }
}
