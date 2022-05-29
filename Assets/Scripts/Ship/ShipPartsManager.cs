using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Must be placed on the Base object of the ship
/// </summary>
public class ShipPartsManager : MonoBehaviour, IDamagable
{
    [SerializeField]
    private SpriteRenderer ShipSprite;

    private ShipWeapons _weapons;
    private ShipHealth _health;
    private PowerupInteractor _powerUpInteractor;
    private ShipMotor _motor;

    public ShipWeapons GetWeapons
    {
        get { return _weapons; }
    }
    public ShipHealth GetHealth
    {
        get { return _health; }
    }
    public PowerupInteractor GetPowerupInteractor
    {
        get { return _powerUpInteractor; }
    }
    public ShipMotor GetMotor
    {
        get { return _motor; }
    }

    void Awake()
    {
        FindShipComponents();
    }
    /// <summary>
    /// Tries to get all the ship components.
    /// When getting Components check for NULL since they only return if the ship has that component attached
    /// </summary>
    public void FindShipComponents()
    {
        _weapons = GetComponentInChildren<ShipWeapons>();
        _health = GetComponentInChildren<ShipHealth>();
        _powerUpInteractor = GetComponentInChildren<PowerupInteractor>();
        _motor = GetComponent<ShipMotor>();
    }

    public void TakeDamage(ShipWeapons _source)
    {
        if (_health)
        {
            _health.TakeDamage(_source);
        }
    }

    public void SwapShipSprite(Sprite _newSprite)
    {
        if(ShipSprite)
        {
            ShipSprite.sprite = _newSprite;
        }
        else
        {
            Debug.LogError("No ShipSprite has been assigned on " + gameObject.name);
        }
    }
}
