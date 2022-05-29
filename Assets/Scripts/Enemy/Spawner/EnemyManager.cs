using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyManager : MonoBehaviour
{
    [Tooltip("Enemytypes will be randomly chosen.")]
    public GameObject[] EnemyShipPrefabs;

    [Space]
    public SO_SpawnBehavior EnemySpawnBehavior;
    public bool CanSpawnMoreEnemies
    {
        get { return _currentAmountOfEnemies < _maxAmountOfEnemies; }
    }

    [SerializeField]
    private int _maxAmountOfEnemies;
    private int _currentAmountOfEnemies;

    [SerializeField]
    private bool spawnEnemies;
    public bool SpawnEnemies
    {
        get { return spawnEnemies; }
        set
        {
            spawnEnemies = value;
            if (value == true)
            {
                CallSpawnFunction();
            }
        }
    }

    private List<ShipHealth> _currentEnemies = new List<ShipHealth>();
    private List<StateController> _currentEnemiesStateControllers = new List<StateController>();

    public delegate void OnEnemySpawned(ShipHealth _enemy);
    public OnEnemySpawned OnEnemySpawnedEvent;

    private HighscoreManager _highScoreManager;
    
    private void Awake()
    {
        OnEnemySpawnedEvent += OnNewEnemySpawned;
    }

    private void OnNewEnemySpawned(ShipHealth _enemy)
    {
        //ceate a new list if it is not initialized
        if (_currentEnemies == null) _currentEnemies = new List<ShipHealth>();
        if (_currentEnemiesStateControllers == null) _currentEnemiesStateControllers = new List<StateController>();

        //add enemy to the list
        _currentEnemies.Add(_enemy);
        _currentEnemiesStateControllers.Add(_enemy.GetComponent<StateController>());

        //subscribe to his ondeath event
        _enemy.OnShipDeath += OnEnemyDeath;
        _currentAmountOfEnemies++;
    }
    private void OnEnemyDeath(ShipHealth _enemy)
    {
        if (_currentEnemies.Contains(_enemy))
        {
            _enemy.OnShipDeath = null;
            _currentEnemies.Remove(_enemy);

            StateController enemyStateCon = _enemy.GetComponent<StateController>();
            if (enemyStateCon)
            {
                AddScoreToHighScore(enemyStateCon);
                _currentEnemiesStateControllers.Remove(enemyStateCon);
            }

            _currentAmountOfEnemies--;
        }
    }


    public void DestroyAllActiveAI()
    {
        if (_currentAmountOfEnemies > 0 && _currentEnemies != null)
        {
            foreach (ShipHealth enemy in _currentEnemies)
            {
                if (enemy != null)
                    Destroy(enemy.gameObject);
            }
        }
        _currentAmountOfEnemies = 0;
        _currentEnemies = new List<ShipHealth>();
        _currentEnemiesStateControllers = new List<StateController>();
    }
    public void ChangeAllAIActiveState(bool _activestate)
    {
        SpawnEnemies = _activestate;
        foreach (StateController _stateCon in _currentEnemiesStateControllers)
        {
            _stateCon.SetAiActiveState(_activestate);
        }
    }


    public void SpawnSingleEnemy()
    {
        SpawnEnemies = true;
        SpawnEnemies = false;
    }
    public GameObject GetRandomEnemyPrefab()
    {
        return EnemyShipPrefabs[UnityEngine.Random.Range(0, EnemyShipPrefabs.Length)];
    }
    private void CallSpawnFunction()
    {
        if (EnemySpawnBehavior)
        {
            StartCoroutine(EnemySpawnBehavior.SpawnBehavior(this));
        }
    }


    public void SetHighSoreManager(HighscoreManager _highScoreManager)
    {
        if (_highScoreManager != null)
        {
            this._highScoreManager = _highScoreManager;
        }
    }
    private void AddScoreToHighScore(StateController _enemy)
    {
        //check if we have everything we need to add a new score
        if (_enemy != null && _enemy.EnemyStats && _highScoreManager)
        {
            //change the highscore with the enemy scoreOnDeath
            if (_highScoreManager.OnHighScoreChanged != null)
                _highScoreManager.OnHighScoreChanged(_enemy.EnemyStats.ScoreAddedOnDeath);
        }
    }
}