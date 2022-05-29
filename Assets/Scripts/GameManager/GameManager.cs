using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    running,
    paused,
    gameOver
}

public class GameManager : MonoBehaviour
{
    [Space]
    [Tooltip("Prefab that has the PlayerShipManager.cs attached.")]
    public PlayerManager PlayerControllerPrefab;
    [Tooltip("Ui prefab with the pausemenu and the Game score.")]
    public GameObject GameUIPrefab;
    private GameObject _gameUIInstance;


    [Tooltip("Camera used when all players are dead.")]
    public GameObject SceneCameraObject;
    public AudioClip GameOverAudioClip;

    /// <summary>
    /// returns a list of all registered players in the game
    /// </summary>
    public List<PlayerManager> GetAllPlayers
    {
        get { return players; }
    }
    private List<PlayerManager> players;

    public static GameManager Instance;
    private EnemyManager _enemyManager;
    private PowerUpSpawnManager _powerUpSpawner;

    public delegate void OnGameOverDelegate();
    public OnGameOverDelegate OnGameOver;

    public EGameState GameState = EGameState.gameOver;

    private PlayersInGameHook _playersInGame;
    private MusicManager _musicManager;
    private HighscoreManager _highScoreManager;
    public HighscoreManager HighscoreManager
    {
        get { return _highScoreManager; }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _playersInGame = PlayersInGameHook.Instance;
        _enemyManager = GetComponent<EnemyManager>();
        _musicManager = MusicManager.Instance;
        _powerUpSpawner = GetComponent<PowerUpSpawnManager>();
        _highScoreManager = GetComponent<HighscoreManager>();

        //initialize the game with the playersInGame
        if (_playersInGame != null)
        {
            InitializeGame();
        }
        else
        {
            Debug.LogError("WE HAVE NO INSTANCE OF PLAYERSINGAMEHOOK. CANNOT INITIALIZE GAME");
            return;
        }
    }

    public bool InitializeGame()
    {
        //parent objects to clean up hirarchy//////////////////
        GameObject playerControllerParent = new GameObject();
        playerControllerParent.name = "playerControllerParent";
        playerControllerParent.transform.SetParent(transform);
        ///////////////////////////////////////////////////////

        //create game ui if we have a prefab to spawn from  
        if (GameUIPrefab)
        {
            _gameUIInstance = Instantiate(GameUIPrefab, transform);
        }

        //check for spawnable prefab
        if (PlayerControllerPrefab)
        {
            //delete all previous players
            if (players != null)
                DeleteAllPlayers();

            //create new playerlist
            players = new List<PlayerManager>();

            //spawn amount of players
            for (int i = 0;  i < _playersInGame.PlayersInGame.Length; i++)
            {
                //Spawn player and add to the list
                players.Add(Instantiate(PlayerControllerPrefab, playerControllerParent.transform));
                //initialize the player
                //TODO: Change to custom color
                players[i].Initialize(GetSpawnPosition(_playersInGame.PlayersInGame.Length, i), _playersInGame.PlayersInGame[i]);
            }
            StartGame();
            InitializeHighScoreManager();
            return true;
        }
        else
            Debug.LogError("PlayerControllerPrefab has not been set on GameManager.cs");

        return false;
    }

    private Vector2 GetSpawnPosition(int _maxPlayers, int _currentPlayer)
    {
        return new Vector2(-_maxPlayers + _currentPlayer, 0);
    }

    /// <summary>
    /// Starts the game
    /// Enables all player input
    /// Starts AI Spawning
    /// Starts AI Behavior
    /// </summary>
    public void StartGame()
    {
        if (_enemyManager)
        {
            _enemyManager.SpawnEnemies = true;
            _enemyManager.ChangeAllAIActiveState(true);
        }

        SetPowerUpSpawning(true);
        SetAllPlayersInputState(true);
        ToggleSceneCamera(false);
        //get music manager if we don't already have it
        if (_musicManager == null)
            _musicManager = MusicManager.Instance;

        //check again since we might not have one in the scene
        if (_musicManager)
        {
            _musicManager.StartMusicPlay();
        }

        GameState = EGameState.running;
    }

    public void RestartGame()
    {
        if (_gameUIInstance)
            Destroy(_gameUIInstance);

        InitializeGame();
    }

    /// <summary>
    /// Stops the game
    /// Disables all player input
    /// Stops AI from spawning
    /// Stops AI behavior
    /// </summary>
    public void InitializeGameOver()
    {
        if (_enemyManager)
        {
            //stop enemy spawning 
            _enemyManager.SpawnEnemies = false;
            //destroy all active AI
            _enemyManager.DestroyAllActiveAI();
        }

        //destroy all registered players
        DeleteAllPlayers();

        GameState = EGameState.gameOver;
        ToggleSceneCamera(true);

        //get music manager if we don't already have it
        if (_musicManager == null)
            _musicManager = MusicManager.Instance;

        //check again since we might not have one in the scene
        if (_musicManager && GameOverAudioClip)
        {
            _musicManager.PlaySpecialClip(GameOverAudioClip, false);
        }

        if (OnGameOver != null)
            OnGameOver();
        //TODO: Save score etc..
    }

    /// <summary>
    /// Pauses the game
    /// Disables all player input
    /// Stops AI from spawning
    /// Stops AI behavior
    /// </summary>
    public void PauseGame()
    {
        _enemyManager.SpawnEnemies = false;
        _enemyManager.ChangeAllAIActiveState(false);
        SetAllPlayersInputState(false);
        GameState = EGameState.paused;
    }

    /// <summary>
    /// Resumes the game
    /// Enables all player input
    /// Starts AI Spawning
    /// Starts AI Behavior
    /// </summary>
    public void ResumeGame()
    {
        _enemyManager.SpawnEnemies = true;
        _enemyManager.ChangeAllAIActiveState(true);
        SetAllPlayersInputState(true);
        GameState = EGameState.running;
    }

    /// <summary>
    /// removes all players from the dictionary and deletes all related gameobjects
    /// </summary>
    public void DeleteAllPlayers()
    {
        if (players != null && players.Count > 0)
        {
            foreach (PlayerManager player in players)
            {
                if (player != null)
                {
                    //destroy all related objects
                    player.DestroyPlayer();
                }
            }
        }
        players = new List<PlayerManager>();
    }

    /// <summary>
    /// Gets a random player in the game that is alive aka. is controlling an intact ship.
    /// Can return null.
    /// </summary>
    /// <returns>If no alive player is found returns null</returns>
    public GameObject GetRandomPlayerInGame()
    {
        if (players.Count > 0)
        {
            GameObject player = null;
            do
            {
                int num = Random.Range(0, players.Count);
                if (players[num].IsAlive && players[num].GetControlledShip != null)
                {
                    player = players[num].GetControlledShip;
                }
            }
            while (player == null && GetNumAlivePlayers() > 0);
            return player;
        }
        else
            return null;
    }

    /// <summary>
    /// returns number of players that are currently alive
    /// </summary>
    /// <returns></returns>
    private int GetNumAlivePlayers()
    {
        int alive = 0;
        foreach (PlayerManager player in players)
        {
            if (player.IsAlive)
                alive++;
        }
        //stop all AI if no players are alive
        if (alive == 0)
        {
            InitializeGameOver();
        }
        return alive;
    }

    /// <summary>
    /// Disables/Enables input for all registered players
    /// </summary>
    private void SetAllPlayersInputState(bool _inputstate)
    {
        foreach (PlayerManager player in players)
        {
            player.CanControllShip = _inputstate;
        }
    }

    private void ToggleSceneCamera(bool _activeState)
    {
        if (SceneCameraObject)
        {
            SceneCameraObject.SetActive(_activeState);
        }
    }

    private void SetPowerUpSpawning(bool _activeState)
    {
        if (_powerUpSpawner)
            _powerUpSpawner.SpawnPowerUp = _activeState;
    }

    private void InitializeHighScoreManager()
    {
        if (_highScoreManager)
        {
            _highScoreManager.Initialize(this);
            _enemyManager.SetHighSoreManager(_highScoreManager);
        }
    }
}