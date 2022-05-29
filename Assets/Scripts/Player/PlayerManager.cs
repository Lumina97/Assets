using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject DefaultShip;
    public GameObject ChangeDefaultShip
    {
        set { DefaultShip = value; }
    }

    [SerializeField]
    private GameObject ShipUIPrefab;
    private ShipUIManager _shipUIInstance;

    [SerializeField]
    private CameraManager CameraPrefab;
    private CameraManager _camera;

    public bool IsAlive
    {
        get { return _controlledShipObject != null; }
    }

    private bool _canControlShip;
    public bool CanControllShip
    {
        get { return _canControlShip; }
        set
        {
            _canControlShip = value;
            if (_input)
            {
                _input.ReceiveInput = value;
            }
        }
    }

    private GameObject _controlledShipObject;
    public GameObject GetControlledShip
    {
        get { return _controlledShipObject; }
        protected set { _controlledShipObject = value; }
    }

    public delegate void OnPlayerIsAliveChangedDelegate(PlayerManager _player, bool _isAlive);
    public OnPlayerIsAliveChangedDelegate OnPlayerIsAliveChanged;

    private ShipPartsManager _shipParts;
    private ShipInput _input;
    private ShipHealth _health;

    private PlayerGameSettingsHook _playerSettings;

    public bool Initialize(Vector2 _spawnPosition, PlayerGameSettingsHook _playerSettings)
    {
        //save player settings
        this._playerSettings = _playerSettings;

        //use name if we have one
        if (_playerSettings.PlayerName != null)
        {
            gameObject.name = _playerSettings.PlayerName;
        }
        else
        {
            //otherwise use the playernumber
            gameObject.name = "Player " +  _playerSettings.PlayerNumber.ToString();
        }

        _input = GetComponent<ShipInput>();
        //check if we found input
        //if no then throw error
        if (_input == null)
        {
            Debug.LogError("PlayerShipManager.cs has not found ShipInput.cs on - " + gameObject.name);
            return false;
        }
        _input.ReceiveInput = true;
        _input.SetInputScheme(_playerSettings.Controls);

        if (DefaultShip)
        {
            //spawn a ship on initialisation
            _controlledShipObject = Instantiate(DefaultShip, _spawnPosition, Quaternion.identity);

            //check for parts manager and return false 
            //if it has not been found
            _shipParts = _controlledShipObject.GetComponent<ShipPartsManager>();
            if (_shipParts == null)
            {
                Debug.LogError("Ship has no parts manager attached to it!");
                return false;
            }

            if (_controlledShipObject)
            {
                return ChangeControlledShip(_controlledShipObject);
            }
            else
            {
                Debug.LogError("Instantiation of Defaultship has failed! PlayerShipManager.cs - Initialize() on - " + gameObject.name);
                return false;
            }
        }
        else
        {
            Debug.LogError("No default ship gameobject has been assigned to PlayerShipManager.cs");
            return false;
        }
    }

    private bool SetupPlayerUI()
    {
        if (ShipUIPrefab)
        {
            //spawn UI as child of this gameobject
            //cleans up the Hirarchy a bit and has player stuff in one place
            _shipUIInstance = Instantiate(ShipUIPrefab, transform).GetComponent<ShipUIManager>();
            //set name
            //TODO: let players set names
            _shipUIInstance.gameObject.name = gameObject.name + "'s - UI";
            _shipUIInstance.Initialize(_playerSettings, _shipParts.GetMotor, _shipParts.GetHealth);
        }
        else
        {
            Debug.LogError("No PlayerUiInstance has been assigned. Playermanager.cs -> SetupPlayerUI() on" + gameObject.name);
            return false;
        }
        return true;
    }

    private bool SetupPlayerCamera()
    {
        if (CameraPrefab)
        {
            _camera = Instantiate(CameraPrefab);
            _camera.SetTarget(_controlledShipObject);
            if (_shipParts.GetHealth)
            {
                _shipParts.GetHealth.OnShipHealthChanged += ExecuteCameraShakeOnShipDamage;
                return true;
            }
            return false;
        }
        else
        {
            Debug.LogError("PlayerManager.cs has no camera prefab reference. Please add one. on " + gameObject.name);
            return false;
        }
    }

    public void ExecuteCameraShakeOnShipDamage(int _ShipLives, bool _tookDamage)
    {
        if (_tookDamage)
            //add 0.01 so we never devide by 0
            _camera.ShakeCamera(100 / (_ShipLives + 0.01f), .05f);
    }

    public bool ChangeControlledShip(GameObject _newShip)
    {
        if (_newShip != null)
        {
            _controlledShipObject = _newShip;
            _controlledShipObject.gameObject.name = _playerSettings.PlayerName + "'s Ship";
            //pass components to the input if we already have an input
            if (_input)
            {
                //get the motor
                _input.Motor = _shipParts.GetMotor;
                if (_input.Motor == null)
                    return false;

                //get the weapons
                _input.Weapons = _shipParts.GetWeapons;
                if (_input.Weapons == null)
                    return false;

                //get ship health
                _health = _shipParts.GetHealth;
                if (_health)
                { 
                    _health.OnShipDeath += OnShipDeath;
                    if(_playerSettings.PlayerDeathParticlePrefab != null)
                    {
                        _health.DeathParticle = _playerSettings.PlayerDeathParticlePrefab;
                    }
                }
                _shipParts.SwapShipSprite(_playerSettings.PlayerSprite);

                SetupPlayerUI();
                SetupPlayerCamera();
                _input.Motor.OnFatalVelocityChange += FatalVelocityChange;
            }
            else
                Debug.LogError("No Input has been set on PlayerShipManager.cs -> ChangeControlledShip() on - " + gameObject.name);
        }
        else
        {
            //if newship == null disable input
            if (_input)
                _input.ReceiveInput = false;

            Debug.LogError("The new ship that was passed in was null! Playermanager.cs -> ChangeControlledShip() on " + gameObject.name);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Destroys all related gameobjects to this player.
    /// </summary>
    public void DestroyPlayer()
    {
        //destroy the controlled ship if we have one
        if (GetControlledShip != null)
            DestroyImmediate(GetControlledShip);
        //destroy UI if we have one
        if (_shipUIInstance)
            DestroyImmediate(_shipUIInstance.gameObject);

        if (_camera)
            DestroyImmediate(_camera.gameObject);

        DestroyImmediate(gameObject);
    }

    private void OnShipDeath(ShipHealth _health)
    {
        //disable input
        if (_input)
            _input.ReceiveInput = false;

        //reset controlled ship to null
        _controlledShipObject = null;

        //destroy Ship Ui 
        //gets spawned when we respawn/get assigned a new ship
        if (_shipUIInstance)
            Destroy(_shipUIInstance.gameObject);

        //Call callback to let subscribe methods know our ship has been destroyed
        if (OnPlayerIsAliveChanged != null)
            OnPlayerIsAliveChanged(this, false);
    }

    /// <summary>
    /// Sbuscribes to the motor delegate to get notified when we have a fatal velocity change 
    /// so we can destroy the ship
    /// </summary>
    private void FatalVelocityChange()
    {
        if (_health)
        {
            _health.TakeDamage();
        }
    }
}