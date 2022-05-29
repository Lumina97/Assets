using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship/Weapon/Automatic/Single Missile")]
public class SO_Automatic_SingleBullet : SO_ShipWeaponType
{
    public override IEnumerator Fire(GameObject _gunPoint, Missile_Base _missile, ShipWeapons _shipWeapons)
    {
        if (_missile)
        {
            //Spawn Missle
            GameObject missileObj = Instantiate(_missile, _gunPoint.transform.position, _gunPoint.transform.rotation).gameObject;
            //Get missle script and set parent
            Missile_Base missile = missileObj.GetComponent<Missile_Base>();
            missile.ParentShip = _shipWeapons;

            if (ShotAudioClip)
            {
                _shipWeapons.AudioSrc.PlayOneShot(ShotAudioClip);
            }
        }
        else
        {
            Debug.LogError("Firetype has not been passed a valid missile");
            yield break;
        }
        yield return null;
    }
}