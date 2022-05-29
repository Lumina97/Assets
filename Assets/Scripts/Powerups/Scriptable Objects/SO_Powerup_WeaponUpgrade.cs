using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUp/Weapon/WeaponType")]
public class SO_Powerup_WeaponUpgrade : SO_Powerup
{
    [Space]
    [Header("WeaponType")]
    [Tooltip("The firetype the player receives on pickup.")]
    public SO_ShipWeaponType UpgradeFireType;

    protected ShipWeapons _weapons;

    public override bool CanUsePowerUpOnShip(GameObject _ship)
    {
        if (_ship != null)
        {
            ShipPartsManager _parts = _ship.GetComponentInParent<ShipPartsManager>();
            _weapons = _parts.GetWeapons;
            if (_weapons)
            {
                //check if the player arleady has an upgrade
                return !_weapons._hasWeaponTypeUpgrade;
            }
        }
        return false;
    }

    protected override void StartEffect()
    {
        if (_ship != null)
        {
            ShipPartsManager _parts = _ship.GetComponentInParent<ShipPartsManager>();
            _weapons = _parts.GetWeapons;
            if (_weapons)
            {
                //else we apply our upgrade as usual
                _weapons.CurrentPrimaryWeapon = UpgradeFireType;
            }
            else
            {
                Debug.LogError("Given ship: " + _ship.name + " has no ShipWeapons.cs attached.");
                return;
            }
        }
    }

    protected override void StopEffect()
    {
        if (_weapons)
        {
            //set to null since the property then sets it back to the default
            _weapons.CurrentPrimaryWeapon = null;
        }
    }
}