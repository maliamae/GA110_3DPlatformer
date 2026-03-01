using UnityEngine;
using static Collectible;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Pickup Sounds")]
    [SerializeField] private AudioClip lightPickupSound;
    [SerializeField] private AudioClip playerJumpSound;
    [SerializeField] private AudioClip playerDashSound;

    [SerializeField] private AudioSource audioSource; //audio source for oneshots (jump, dash, collectible collected, etc)
    [SerializeField] private AudioSource walkAudioSource; //looping audio source for walking SFX

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

    //plays the light pickup sound once
    private void PlayPickupSound(CollectibleType type, int amount)
    {
        switch (type)
        {
            case CollectibleType.Light:
                audioSource.PlayOneShot(lightPickupSound);
                break;
        }
    }

    //plays the player walking sound and loops if player keeps walking
    private void PlayWalkSound(float moveSpeed, bool isPossible)
    {
        if (moveSpeed > 0.1f && !walkAudioSource.isPlaying && isPossible) //checks if the player is walking and the audio source is not currently playing the walking sound
        {
            walkAudioSource.Play();
        }
        else if (moveSpeed < 0.1f && walkAudioSource.isPlaying) //checks if the walking sound is currently being played and the player is not walking
        {
            walkAudioSource.Pause();
        }
        else if (!isPossible) //checks if it is possible for the player to be walking (if they are grounded and not climbing)
        {
            walkAudioSource.Pause();
        }
        //Debug.Log("MoveSpeed: " + moveSpeed);
    }

    //plays jump sound once
    private void PlayPlayerJumpSound()
    {
        audioSource.PlayOneShot(playerJumpSound);
    }

    //plays dash sound once
    private void PlayPlayerDashSound()
    {
        audioSource.PlayOneShot(playerDashSound);
    }
}
