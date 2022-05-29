using UnityEngine;

public class UIShipSelectManager : MonoBehaviour
{
    public SO_ShipColorVariation[] PlayerShipSprites = new SO_ShipColorVariation[3];
    public SO_ControlScheme[] PlayerControls;

    [Space]
    public UIPlayerShipCustomizer PlayerCustomizerPrefab;

    private PlayersInGameHook _playersInGameInstance;
    private void Awake()
    {
        for (int i = 0; i < PlayerShipSprites.Length; i++)
        {
            PlayerShipSprites[i].VariationPos = i;
        }
    }

    public void InitializePlayers(int _amountOfPlayers)
    {
        //check for prefab
        if (PlayerCustomizerPrefab == null)
        {
            Debug.LogError("No Player customizerPrefab has been assigned on - " + gameObject.name);
            return;
        }

        //if we have no reference then check if there is a instance in the game 
        //if so we save that as the reference
        if (_playersInGameInstance == null && PlayersInGameHook.Instance != null)
        {
            _playersInGameInstance = PlayersInGameHook.Instance;
        }
        //clean up if we have an instance
        else if (_playersInGameInstance != null)
        {
            CleanUpPlayers();
        }
        //check if we already have a hook for all the players in the game
        //if no we create one
        else if (_playersInGameInstance == null)
        {
            _playersInGameInstance = new GameObject().AddComponent<PlayersInGameHook>();
            _playersInGameInstance.gameObject.name = "Players Game Settings Hook";
        }

        //create a new array of players
        _playersInGameInstance.PlayersInGame = new PlayerGameSettingsHook[_amountOfPlayers];
        //set it to not destroy on load so we can get the information in the new scene
        DontDestroyOnLoad(_playersInGameInstance.gameObject);

        //_______create all the players_______
        for (int i = 0; i < _amountOfPlayers; i++)
        {
            //create new playershipCustomizer
            UIPlayerShipCustomizer player = Instantiate(PlayerCustomizerPrefab,transform);
            //create a new hook for that player and add it to the array at the same time
            _playersInGameInstance.PlayersInGame[i] = new PlayerGameSettingsHook();
            _playersInGameInstance.PlayersInGame[i].PlayerNumber = i + 1;
            //initialize the playercustomizer for that player and pass in the newly created hook
            player.Initialize(this, PlayerShipSprites[0], i, _playersInGameInstance.PlayersInGame[i]);      
            //set controls for the player
            _playersInGameInstance.PlayersInGame[i].Controls = GetControlsForPlayer(i);
        }
    }

    private SO_ControlScheme GetControlsForPlayer(int _playerNum)
    {
        if (_playerNum == PlayerControls.Length) _playerNum = 0;
        else if (_playerNum < 0) _playerNum = PlayerControls.Length - 1;

        return PlayerControls[_playerNum];
    }

    public void CleanUpPlayers()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    public SO_ShipColorVariation GetShipVariation( ref int _desiredVariation)
    {
        //check if the desiredVariation is viable if not make it so
        if (_desiredVariation < 0) _desiredVariation = PlayerShipSprites.Length - 1;
        else if (_desiredVariation == PlayerShipSprites.Length) _desiredVariation = 0;

        //return next variation
        return PlayerShipSprites[_desiredVariation];
    }
}
