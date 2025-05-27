using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] UnityEngine.UI.Scrollbar musicVolumeSlider;
    [SerializeField] float musicVolume;
    [SerializeField] float musicVolumeMax;

    void Update()
    {
        if (musicVolumeSlider != null)
        {
            musicVolume = musicVolumeSlider.value * musicVolumeMax;
        }
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = musicVolume;
        }
    }
}
