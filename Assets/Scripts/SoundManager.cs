using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; } // Singleton instance

    public AudioClip jumpSound; // Sound for jumping
    public AudioSource walkingSound; // Sound for walking
    public AudioSource zipSound;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed when loading a new scene
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>(); // Create a new AudioSource component
        audioSource.clip = clip; // Set the audio clip
        audioSource.volume = 0.2f; // Set the volume (0.0 to 1.0)
        audioSource.Play(); // Play the sound
        Destroy(audioSource, clip.length); // Destroy the AudioSource after the clip has finished playing

    }

    public void PlaySoundWalk()
    {
        if (walkingSound.isPlaying) return; // Prevent overlapping sounds
        walkingSound.loop = true; // Loop the sound
        walkingSound.Play(); // Play the walking sound
    }

    public void StopSoundWalk()
    {
        walkingSound.Stop(); // Stop the walking sound
    }

    public void PlaySoundZip()
    {
        if (zipSound.isPlaying) return; // Prevent overlapping sounds
        zipSound.loop = true; // Loop the sound
        zipSound.Play(); // Play the walking sound
    }

    public void StopSoundZip()
    {
        zipSound.Stop(); // Stop the walking sound
    }
}

