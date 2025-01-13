using UnityEngine;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenuButtonsObj;
    public UILeaderboardsManager LeaderboardsManager;
    public GameObject ControlsObject;
    public UIOptionsManager SettingsObject;
    public UIGameOptionsManager GameOptionsManager;

    [Space]
    [Header("Audio")]
    public AudioMixer MainAudioMixer;
    public SO_AudioSettings AudioSettingsToLoadFrom;

    private void Start()
    {
        if (SettingsHelper.Instance)
            SettingsHelper.Instance.LoadAudioSettings();
    }

    private void OnEnable()
    {
        if (MainMenuButtonsObj) MainMenuButtonsObj.SetActive(true);
        if (ControlsObject) ControlsObject.SetActive(false);
        if (SettingsObject) SettingsObject.gameObject.SetActive(false);
        if (GameOptionsManager) GameOptionsManager.gameObject.SetActive(false);
    }

    public void OpenControls()
    {
        MainMenuButtonsObj.SetActive(false);
        ControlsObject.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        LeaderboardsManager.gameObject.SetActive(false);
        MainMenuButtonsObj.SetActive(true);
        ControlsObject.SetActive(false);
    }

    public void StartGame()
    {
        if (GameOptionsManager)
        {
            GameOptionsManager.gameObject.SetActive(true);
            GameOptionsManager.OnGameOptionsClosed += OnGameOptionsClosed;
            if (MainMenuButtonsObj) MainMenuButtonsObj.SetActive(false);
        }
    }

    public void QuitGame()
    {
        if (SettingsHelper.Instance)
            SettingsHelper.Instance.SaveAudioSettings();

        CallExternalFunction("onUnityQuit");
        Application.Quit();
    }

    private void CallExternalFunction(string functionName)
    {
#if UNITY_WEBGL && !UNITY_EDITOR        
        Application.ExternalEval($"{functionName}();");
#endif
    }

    public void OpenSettings()
    {
        if (SettingsObject)
        {
            SettingsObject.gameObject.SetActive(true);
            SettingsObject.OnSettingsClosed += OnSettingsClosed;
        }
    }

    public void OpenLoaderboards()
    {
        if(LeaderboardsManager)
        {
            if (MainMenuButtonsObj) MainMenuButtonsObj.SetActive(false);
            LeaderboardsManager.gameObject.SetActive(true);
        }
    }

    public void OnSettingsClosed()
    {
        SettingsObject.OnSettingsClosed -= OnSettingsClosed;
        SettingsObject.gameObject.SetActive(false);

        if (MainMenuButtonsObj) MainMenuButtonsObj.SetActive(true);
        if (ControlsObject) ControlsObject.SetActive(false);
    }

    public void OnGameOptionsClosed()
    {
        GameOptionsManager.OnGameOptionsClosed -= OnGameOptionsClosed;
        if (MainMenuButtonsObj) MainMenuButtonsObj.SetActive(true);
    }
}
