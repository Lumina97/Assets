using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerShipCustomizer : MonoBehaviour
{
    public Image PlayerShipSprite;
    public Image PlayerNameInputFieldBackground;

    private PlayerGameSettingsHook _playerSettingsHookInstance;
    private SO_ShipColorVariation _currentShipColor;
    private UIShipSelectManager _shipSelectManager;

    private int _currentColor = 0;
    private int _currentStyle = 0;

    public void Initialize(UIShipSelectManager _shipSelectManager, SO_ShipColorVariation _currentShipVariation, int _defaultColor, PlayerGameSettingsHook _playerHook)
    {
        //save references
        this._currentShipColor = _currentShipVariation;
        this._shipSelectManager = _shipSelectManager;
        //save color
        _currentColor = _defaultColor;
        // check if it is valid
        if (_currentColor > _currentShipVariation.ShipSprites.Length) _currentColor = 0;
        else if (_currentColor < 0) _currentColor = _currentShipVariation.ShipSprites.Length;

        _playerSettingsHookInstance = _playerHook;

        //update ui elements
        UpdateUIElements();
    }

    public void ChangePlayerName(string _newName)
    {
        if (_playerSettingsHookInstance != null)
        {
            _playerSettingsHookInstance.PlayerName = _newName;
        }
    }

    //---------- Change color ----------
    public void ChangeColorLeft()
    {
        _currentColor--;
        _currentShipColor = _shipSelectManager.GetShipVariation(ref _currentColor);
        UpdateUIElements();
    }
    public void ChangeColorRight()
    {
        _currentColor++;
        _currentShipColor = _shipSelectManager.GetShipVariation(ref _currentColor);
        UpdateUIElements();
    }

    //---------- Change variation ----------
    public void ChangeShipVariationLeft()
    {
        _currentStyle--;
        //check if its a valid style
        if (_currentStyle < 0) _currentStyle = _currentShipColor.ShipSprites.Length - 1;

        UpdateUIElements();
    }
    public void ChangeShipVariationRight()
    {
        _currentStyle++;
        //check if its a valid style
        if (_currentStyle >= _currentShipColor.ShipSprites.Length) _currentStyle = 0;

        UpdateUIElements();
    }

    private void UpdateUIElements()
    {
        if (_currentShipColor)
        {
            if (PlayerShipSprite)
            {
                PlayerShipSprite.sprite = _currentShipColor.ShipSprites[_currentStyle];
                if (_playerSettingsHookInstance != null)
                {
                    _playerSettingsHookInstance.PlayerSprite = PlayerShipSprite.sprite;
                    _playerSettingsHookInstance.PlayerLivesSprite = _currentShipColor.ShipLivesSprites[_currentStyle];
                }
            }
            if (PlayerNameInputFieldBackground)
            {
                PlayerNameInputFieldBackground.sprite = _currentShipColor.BackGroundSprites;
                if (_playerSettingsHookInstance != null)
                {
                    _playerSettingsHookInstance.BackgroundSprite = PlayerNameInputFieldBackground.sprite;
                    _playerSettingsHookInstance.PlayerBoosterBarColor = _currentShipColor.BoosterColors;
                }
            }

            // Change the DeathParticle
            //we do it here because #badDesign
            if (_playerSettingsHookInstance != null)
            {
                _playerSettingsHookInstance.PlayerDeathParticlePrefab = _currentShipColor.DeathParticles;
            }
        }
    }
}
