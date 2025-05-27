using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource musicPlayerAudioSource;
    [SerializeField] AudioClip[] availableSongs;

    AudioClip songToPlay;
    AudioClip songThatJustPlayed;

    private void Awake()
    {
        if (FindObjectsByType<MusicPlayer>(FindObjectsSortMode.None).Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (!musicPlayerAudioSource.isPlaying)
        {
            PlaySong();
        }
    }

    public void PlaySong()
    {
        songToPlay = availableSongs[Random.Range(0, availableSongs.Length)];
        if (songToPlay != songThatJustPlayed)
        {
            musicPlayerAudioSource.clip = songToPlay;
            musicPlayerAudioSource.Play();

            songThatJustPlayed = songToPlay;
        }
        else
        {
            PlaySong();
        }
    }
}
