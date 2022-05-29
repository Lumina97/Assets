using UnityEngine;

/// <summary>
/// Holds array of players to create players in the play scene
/// </summary>
public class PlayersInGameHook : MonoBehaviour
{
    //singleton so we can easily acces this script from the playscene
    public static PlayersInGameHook Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    //array of player settings
    public PlayerGameSettingsHook[] PlayersInGame;
}
