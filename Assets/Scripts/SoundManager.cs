using UnityEngine;
using static Collectible;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Pickup Sounds")]
    [SerializeField] private AudioClip lightPickupSound;
    [SerializeField] private AudioClip playerJumpSound;
    [SerializeField] private AudioClip playerDashSound;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource walkAudioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        CollectibleEventSystem.OnCollectibleCollected += PlayPickupSound;
        InputManager.OnPlayerMove += PlayWalkSound;
        InputManager.OnPlayerJump += PlayPlayerJumpSound;
        InputManager.OnPlayerDash += PlayPlayerDashSound;
    }

    private void OnDisable()
    {
        CollectibleEventSystem.OnCollectibleCollected -= PlayPickupSound;
        InputManager.OnPlayerMove -= PlayWalkSound;
        InputManager.OnPlayerJump -= PlayPlayerJumpSound;
        InputManager.OnPlayerDash -= PlayPlayerDashSound;
    }

    private void PlayPickupSound(CollectibleType type, int amount)
    {
        switch (type)
        {
            case CollectibleType.Light:
                audioSource.PlayOneShot(lightPickupSound);
                break;
        }
    }

    private void PlayWalkSound(float moveSpeed, bool isPossible)
    {
        if (moveSpeed > 0.1f && !walkAudioSource.isPlaying && isPossible)
        {
            walkAudioSource.Play();
        }
        else if (moveSpeed < 0.1f && walkAudioSource.isPlaying)
        {
            walkAudioSource.Pause();
        }
        else if (!isPossible)
        {
            walkAudioSource.Pause();
        }
        Debug.Log("MoveSpeed: " + moveSpeed);
    }

    private void PlayPlayerJumpSound()
    {
        audioSource.PlayOneShot(playerJumpSound);
    }

    private void PlayPlayerDashSound()
    {
        audioSource.PlayOneShot(playerDashSound);
    }
}
