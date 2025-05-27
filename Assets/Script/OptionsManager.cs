using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] UnityEngine.UI.Scrollbar musicVolumeSlider;
    [SerializeField] float musicVolumeMax;

    [Header("SFX")]
    [SerializeField] UnityEngine.UI.Scrollbar SFXVolumeSlider;
    [SerializeField] float SFXVolumeMax;

    AudioSource musicAudioSource;
    AudioSource[] SFXAudioSources;
    float musicVolume;
    float SFXVolume;

    private void Awake()
    {
        if (FindObjectsByType<OptionsManager>(FindObjectsSortMode.None).Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Update()
    {   
        //Music
        MusicPlayer musicPlayer = GetComponent<MusicPlayer>();
        if (musicPlayer != null)
        {
            musicAudioSource = musicPlayer.GetComponent<AudioSource>();
        }

        if (musicVolumeSlider != null)
        {
            musicVolume = musicVolumeSlider.value * musicVolumeMax;
        }
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = musicVolume;
        }

        //SFX
        SoundManager soundManager = FindFirstObjectByType<SoundManager>();
        if (soundManager != null)
        {
            SFXAudioSources = soundManager.GetComponents<AudioSource>();
        }

        if (SFXVolumeSlider != null)
        {
            SFXVolume = SFXVolumeSlider.value * SFXVolumeMax;
        }
        foreach (AudioSource SFXAudioSource in SFXAudioSources)
        {
            SFXAudioSource.volume = SFXVolume;
        }
    }
}
