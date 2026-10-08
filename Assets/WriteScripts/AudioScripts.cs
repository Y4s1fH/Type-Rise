using UnityEngine;

public class AudioScripts : MonoBehaviour
{
    public static AudioScripts Instance;

    [Header("Audio Sources")]
    public AudioSource mainmusicSource;
    public AudioSource encounterinputSource;
    public AudioSource clickSource;
    public AudioSource wrongSource;

    [Header("Clips")]
    public AudioClip mainMusic;         
    public AudioClip encounterMusic;    
    public AudioClip clickSound;       

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PlayMainMusic();
    }

    public void PlayClickSound()
    {
        if (clickSource != null && clickSound != null)
            clickSource.PlayOneShot(clickSound);
    }

    public void PlayMainMusic()
    {
        if (mainmusicSource != null && mainMusic != null)
        {
            mainmusicSource.PlayOneShot(mainMusic);
        }
    }
    public void PlayEncounterMusic()
    {
        if (encounterinputSource != null && encounterMusic != null)
        {
            encounterinputSource.PlayOneShot(encounterMusic);
        }
    }
}