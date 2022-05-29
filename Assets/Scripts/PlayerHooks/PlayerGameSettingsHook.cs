using UnityEngine;

/// <summary>
/// simple player data container to get information from menu to playscene
/// </summary>
[System.Serializable]
public class PlayerGameSettingsHook
{
    public Sprite PlayerSprite;
    public Sprite BackgroundSprite;
    public Sprite PlayerLivesSprite;
    public Color PlayerBoosterBarColor;
    public GameObject PlayerDeathParticlePrefab;
    public string PlayerName;
    public SO_ControlScheme Controls;
    public int PlayerNumber;
}
