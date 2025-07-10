using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME_KEY = "MusicVolume";
    // Singleton instance
    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;
    private float volume = .3f;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        volume = GetPrefsDefaultVolume();
    }
    public void ChangeVolume(float receivingVolume)
    {
        audioSource.volume = receivingVolume / 4;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME_KEY, receivingVolume);
        PlayerPrefs.Save();

    }
    
    public float GetPrefsDefaultVolume()
    {
        return PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME_KEY, 0.3f);
    }
}
