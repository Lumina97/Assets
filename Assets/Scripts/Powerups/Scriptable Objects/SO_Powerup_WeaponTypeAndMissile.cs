using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUp/Weapon/WeaponType and Missile")]
public class SO_Powerup_WeaponTypeAndMissile : SO_Powerup_WeaponUpgrade
{
    [Space]
    [Header("Missiletype")]
    public Missile_Base UpgradeMissileType;

    public override bool CanUsePowerUpOnShip(GameObject _ship)
    {
       bool canUse  =  base.CanUsePowerUpOnShip(_ship);
        //check if the base class can use it 
        //and if not then we already return false
        if (canUse == false)
            return false;

        if (_ship != null)
        {
            ShipPartsManager _parts = _ship.GetComponentInParent<ShipPartsManager>();
            _weapons = _parts.GetWeapons;
            if (_weapons)
            {
                //check if the player arleady has an upgrade
                return !_weapons._hasMissileUpgrade;
            }
        }
        return false;
    }

    protected override void StartEffect()
    {
        base.StartEffect();
        _weapons.CurrentMissile = UpgradeMissileType;
    }

    protected override void StopEffect()
    {
        base.StopEffect();

        if (_weapons)
        {
            _weapons.CurrentMissile = null;
        }
    }
}
