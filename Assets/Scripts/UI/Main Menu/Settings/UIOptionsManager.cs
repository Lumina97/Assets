using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptionsManager : MonoBehaviour
{
    public GameObject ButtonsObject;
    public UIAudioManager AudioManager;

    public delegate void OnSettingsClosedDelegate();
    public OnSettingsClosedDelegate OnSettingsClosed;


    private void OnEnable()
    {
        if (ButtonsObject)
            ButtonsObject.SetActive(true);

        if (AudioManager)
            AudioManager.gameObject.SetActive(false);
    }

    public void CloseSettings()
    {
        if (OnSettingsClosed != null)
            OnSettingsClosed();

        gameObject.SetActive( false );
    }

    public void OpenAudioOptions()
    {
        if (AudioManager)
        {
            AudioManager.gameObject.SetActive( true );
            AudioManager.OnCloseAudioSettings += OnAudioSettingsClosed;
            if (ButtonsObject)
                ButtonsObject.SetActive( false );
        }
    }

    private void OnAudioSettingsClosed()
    {
        AudioManager.OnCloseAudioSettings -= OnAudioSettingsClosed;
        if(ButtonsObject)
        {
            ButtonsObject.SetActive( true );
        }
    }
    
}

