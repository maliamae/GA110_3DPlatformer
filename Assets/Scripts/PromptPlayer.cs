using TMPro;
using UnityEngine;

public class PromptPlayer : MonoBehaviour
{
    //text that prompts the player to use the dash mechanic, but is destroyed once the player dashes
    [SerializeField] private TMP_Text promptText;

    private void OnEnable()
    {
        InputManager.OnPlayerDash += DisablePrompt;
    }

    private void OnDisable()
    {
        InputManager.OnPlayerDash -= DisablePrompt;
    }

    private void DisablePrompt()
    {
        //promptText.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
