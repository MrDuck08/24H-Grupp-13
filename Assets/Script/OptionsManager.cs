using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    UnityEngine.UI.Scrollbar musicVolumeSlider;
    float musicVolumeMax = 1;

    UnityEngine.UI.Scrollbar SFXVolumeSlider;
    float SFXVolumeMax = 1;

    AudioSource musicAudioSource;
    AudioSource[] SFXAudioSources;
    float musicVolume;
    float SFXVolume;
    bool foundMusicVolumeSlider = false;
    bool foundSFXVolumeSlider = false;

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
        MusicPlayer musicPlayer = FindFirstObjectByType<MusicPlayer>();
        if (musicPlayer != null)
        {
            musicAudioSource = musicPlayer.GetComponent<AudioSource>();
        }
        if (GameObject.Find("MusicVolumeSlider") != null)
        {
            musicVolumeSlider = GameObject.Find("MusicVolumeSlider").GetComponent<UnityEngine.UI.Scrollbar>();
        }
        
        if (musicVolumeSlider != null)
        {
            musicVolume = musicVolumeSlider.value * musicVolumeMax;
        }
        
        if (musicAudioSource != null && musicVolumeSlider != null)
        {
            musicAudioSource.volume = musicVolume;
            foundMusicVolumeSlider = true;
        }
        if (musicAudioSource != null && musicVolumeSlider == null && foundMusicVolumeSlider == false)
        {
            musicAudioSource.volume = musicVolumeMax / 2;
        }

        //SFX
        SoundManager soundManager = FindFirstObjectByType<SoundManager>();
        if (soundManager != null)
        {
            SFXAudioSources = soundManager.GetComponents<AudioSource>();
        }
        if (GameObject.Find("SFXVolumeSlider") != null)
        {
            SFXVolumeSlider = GameObject.Find("SFXVolumeSlider").GetComponent<UnityEngine.UI.Scrollbar>();
        }
        
        if (SFXVolumeSlider != null)
        {
            SFXVolume = SFXVolumeSlider.value * SFXVolumeMax;
        }
        
        foreach (AudioSource SFXAudioSource in SFXAudioSources)
        {
            
            if (SFXAudioSource != null && SFXVolumeSlider == null && foundSFXVolumeSlider == true)
            {
                SFXAudioSource.volume = SFXVolume;
            }
            else if (SFXAudioSource != null && SFXVolumeSlider == null && foundSFXVolumeSlider == false)
            {
                SFXAudioSource.volume = SFXVolumeMax / 2;
            }
            else if (SFXAudioSource != null && SFXVolumeSlider != null)
            {
                SFXAudioSource.volume = SFXVolume;
                foundSFXVolumeSlider = true;
            } 
        }
    }
}
