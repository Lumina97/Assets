using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIAudioManager : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public SO_AudioSettings AudioSettings;
    public delegate void CloseAudioManagerDelegate();
    public CloseAudioManagerDelegate OnCloseAudioSettings;

    [Space]
    [Header("Sliders")]
    public Slider MusicVolumeSilder;
    public Slider SoundFXVolumeSilder;

    public void CloseAudioMenu()
    {
        if (SettingsHelper.Instance)
            SettingsHelper.Instance.SaveAudioSettings();
        gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        if (OnCloseAudioSettings != null)
            OnCloseAudioSettings();
    }

    private void OnEnable()
    {
        if (AudioSettings)
        {
            if (MusicVolumeSilder) MusicVolumeSilder.value = AudioSettings.MusicVolume;
            if (SoundFXVolumeSilder) SoundFXVolumeSilder.value = AudioSettings.SoundFXVolume;
        }
    }

    public void ChangeMusicVolume(float value)
    {
        if (AudioMixer)
        {
            AudioMixer.SetFloat("MusicVolume", value);
            if (AudioSettings)
                AudioSettings.MusicVolume = value;
        }
    }

    public void ChangeSoundFXVolume(float value)
    {
        if (AudioMixer)
        {
            AudioMixer.SetFloat("SoundFXVolume", value);
            if (AudioSettings)
                AudioSettings.SoundFXVolume = value;
        }
    }
}
