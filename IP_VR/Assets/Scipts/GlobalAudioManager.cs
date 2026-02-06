using UnityEngine;

public class GlobalAudioManager : MonoBehaviour
{
    public static GlobalAudioManager instance;
    private AudioSource musicSource;

    void Awake()
    {
        // If one already exists, destroy this new one
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, make this the official one and keep it forever
        instance = this;
        DontDestroyOnLoad(gameObject);
        musicSource = GetComponent<AudioSource>();
        
        // Load the volume immediately when the game starts
        if (musicSource != null)
            musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    public void ChangeVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume); // Save for next time
        }
    }
}