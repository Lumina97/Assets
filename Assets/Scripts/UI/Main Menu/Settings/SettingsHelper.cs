using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// saves and loads settings
/// </summary>
public class SettingsHelper : MonoBehaviour
{

    #region Audio
    [Space]
    [Header("Audio")]
    public AudioMixer MainAudioMixer;
    public SO_AudioSettings audioSettings;

    public static SettingsHelper Instance;
    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SaveAudioSettings()
    {
        if (audioSettings)
        {
            PlayerPrefs.SetFloat("MusicVolume", audioSettings.MusicVolume);
            PlayerPrefs.SetFloat("SoundFXVolume", audioSettings.SoundFXVolume);
        }
    }

    public void LoadAudioSettings()
    {
        if (audioSettings)
        {
            if (PlayerPrefs.HasKey("MusicVolume") == false)
            {
                PlayerPrefs.SetFloat("MusicVolume", audioSettings.MusicVolume);
            }

            audioSettings.MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            MainAudioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));

            if (PlayerPrefs.HasKey("SoundFXVolume") == false)
            {
                PlayerPrefs.SetFloat("SoundFXVolume", audioSettings.MusicVolume);
            }
            audioSettings.SoundFXVolume = PlayerPrefs.GetFloat("SoundFXVolume");
            MainAudioMixer.SetFloat("SoundFXVolume", PlayerPrefs.GetFloat("SoundFXVolume"));
        }
    }
    #endregion
}