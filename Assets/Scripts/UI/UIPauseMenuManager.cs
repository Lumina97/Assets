using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPauseMenuManager : MonoBehaviour
{
    public GameObject PauseMenuObject;
    public GameObject HUDObject;
    public GameObject GameOverObject;

    private GameManager _gameManager;

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
        _gameManager.OnGameOver += OnGameOver;

    }

    void Update()
    {
        if (_gameManager)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (_gameManager.GameState)
                {
                    case EGameState.running:
                        _gameManager.PauseGame();
                        SetObjectActiveState(PauseMenuObject, true);
                        SetObjectActiveState(HUDObject, false);
                        MouseManager.ToggleMouseLock(false);
                        break;

                    case EGameState.paused:
                        _gameManager.ResumeGame();
                        SetObjectActiveState(PauseMenuObject, false);
                        SetObjectActiveState(HUDObject, true);
                        MouseManager.ToggleMouseLock(true);
                        break;
                }
            }
        }
    }

    public void ResumeGame()
    {

        MouseManager.ToggleMouseLock(true);
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
