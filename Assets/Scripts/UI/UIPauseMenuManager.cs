using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPauseMenuManager : MonoBehaviour
{
    public GameObject PauseMenuObject;
    public GameObject HUDObject;
    public GameObject GameOverObject;

    private GameManager _gameManager;
    private bool _paused;
    private bool _isMobile;
    private MobileControlsManager _mobileControls;

    private void OnEnable()
    {
        MouseManager.ToggleMouseLock(true);
        SetObjectActiveState(PauseMenuObject, false);
        SetObjectActiveState(GameOverObject, false);
        SetObjectActiveState(HUDObject, true);
    }

    void Awake()
    {
        _gameManager = GameManager.Instance;
        if (_gameManager)
        {
            _gameManager.OnGameOver += OnGameOver;
        }

        //Check if we are running on windows or mobile
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _isMobile = true;
            if (MobileControlsManager.Instance != null)
            {
                _mobileControls = MobileControlsManager.Instance;
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            _isMobile = false;
        }
    }

    void Update()
    {
        if (_gameManager)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || CrossPlatformInputManager.GetButton("Pause"))
            {
                switch (_gameManager.GameState)
                {
                    case EGameState.running:
                        _gameManager.PauseGame();
                        SetObjectActiveState(PauseMenuObject, true);
                        SetObjectActiveState(HUDObject, false);

                        if (_isMobile && _mobileControls != null)
                        {
                            _mobileControls.gameObject.SetActive(false);
                        }
                        else if (_isMobile == false)
                        {
                            MouseManager.ToggleMouseLock(false);
                        }

                        break;
                    case EGameState.paused:
                        _gameManager.ResumeGame();
                        SetObjectActiveState(PauseMenuObject, false);
                        SetObjectActiveState(HUDObject, true);

                        if (_isMobile && _mobileControls != null)
                        {
                            _mobileControls.gameObject.SetActive(true);
                        }
                        else if (_isMobile == false)
                        {
                            MouseManager.ToggleMouseLock(true);
                        }

                        break;
                }
            }
        }
    }

    public void ResumeGame()
    {
        if (_isMobile == false)
        {
            MouseManager.ToggleMouseLock(true);
        }
        SetObjectActiveState(PauseMenuObject, false);
        SetObjectActiveState(HUDObject, true);
        _gameManager.ResumeGame();
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void SetObjectActiveState(GameObject _object, bool _activestate)
    {
        if (_object != null)
            _object.SetActive(_activestate);
    }

    void OnGameOver()
    {
        _gameManager.OnGameOver -= OnGameOver;
        if (GameOverObject)
        {
            SetObjectActiveState(HUDObject, false);
            SetObjectActiveState(PauseMenuObject, false);
            SetObjectActiveState(GameOverObject, true);
        }
    }
}
