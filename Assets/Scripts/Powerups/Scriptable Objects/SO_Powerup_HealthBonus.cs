using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUp/Ship/Health Bonus")]
public class SO_Powerup_HealthBonus : SO_Powerup
{
    public int AmountOfHealth = 1;

    public override bool CanUsePowerUpOnShip(GameObject _ship)
    {
        if (_ship != null)
        {
            ShipPartsManager _parts = _ship.GetComponent<ShipPartsManager>();
            if (_parts)
            {
                ShipHealth health = _parts.GetHealth;
                if (health)
                    return health.CurrentLives < health.MaxLives;
            }
        }
        return false;
    }

    protected override void StartEffect()
    {
        if (_ship != null)
        {
            ShipPartsManager _parts = _ship.GetComponent<ShipPartsManager>();
            if (_parts)
            {
                if (_parts.GetHealth)
                    _parts.GetHealth.CurrentLives++;
                else
                    Debug.LogError("The passed in ship has no ShipHealth.cs - SO_Powerup_HealthBonus.cs");
            }
            else
                Debug.LogError("The passed in ship has no ShipParts- SO_Powerup_HealthBonus.cs");
        }

    }

    protected override void StopEffect()
    {
        return;
    }
}
