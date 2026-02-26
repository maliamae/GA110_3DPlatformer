using TMPro;
using UnityEngine;

public class PromptPlayer : MonoBehaviour
{
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
        promptText.gameObject.SetActive(false);
    }
}
