using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource musicAudioSource;

    public AudioClip gunShot;
    public AudioClip testMusic;

    //private void Awake()
    //{
    //    PlayMusic(testMusic);
    //}

    private void Awake()
    {
        if (Object.FindObjectsByType<SoundManager>(FindObjectsSortMode.None).Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        //Checking if any of the AudioSources stopped playing any sound
        //Destroys them if they have
        if (this.gameObject.GetComponent<AudioSource>() != null)
        {
            AudioSource temporaryAudioSource = this.gameObject.GetComponent<AudioSource>();
            if (temporaryAudioSource.isPlaying == false)
            {
                Destroy(temporaryAudioSource);
            }
        }
    }

    public void PlaySound(AudioClip clipToPlay)
    {
        AudioSource temporaryAudioSource = this.gameObject.AddComponent<AudioSource>();
        temporaryAudioSource.playOnAwake = false;
        temporaryAudioSource.clip = clipToPlay;
        temporaryAudioSource.Play();
    }

    public void PlayMusic(AudioClip musicToPlay)
    {
        musicAudioSource.clip = musicToPlay;
        musicAudioSource.Play();
    }
}
