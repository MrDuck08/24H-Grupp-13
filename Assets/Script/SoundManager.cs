using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip gunShot;
    public AudioClip testMusic;

    //private void Awake()
    //{
    //    PlayMusic(testMusic);
    //}

    void Update()
    {
        //Checking if any of the AudioSources stopped playing any sound
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

    void PlayMusic(AudioClip musicToPlay)
    {
        AudioSource musicPlayerAudioSource = this.gameObject.AddComponent<AudioSource>();
        musicPlayerAudioSource.playOnAwake = false;
        musicPlayerAudioSource.loop = true;
        musicPlayerAudioSource.clip = testMusic;
        musicPlayerAudioSource.Play();
    }
}
