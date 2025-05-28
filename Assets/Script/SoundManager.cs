using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [Header("SFX library")]
    public AudioClip gunShotSound;
    public AudioClip hitEnemySound, hitSomthingElseSound;
    public AudioClip hitWoodSound;
    public AudioClip hitSofaSound;
    public AudioClip turnSound;
    public AudioClip droneDestructionSound;
    public AudioClip explosionSound;
    public AudioClip countdownSound;

    private void Awake()
    {
        if (FindObjectsByType<SoundManager>(FindObjectsSortMode.None).Length > 1)
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
}
