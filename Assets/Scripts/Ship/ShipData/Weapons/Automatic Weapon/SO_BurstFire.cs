using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship/Weapon/Automatic/Burst Fire")]
public class SO_BurstFire : SO_ShipWeaponType
{
    public int ShotsPerburst;
    public float BurstShotDelay;
    public override float GetShotDelay
    {
        get
        {
            return ShotDelay + (ShotsPerburst * BurstShotDelay);
        }
    }

    public override IEnumerator Fire(GameObject _gunPoint, Missile_Base _missile, ShipWeapons _shipWeapons)
    {
        for (int i = 1; i <= ShotsPerburst; i++)
        {
            //Spawn Missle
            GameObject missileObj = Instantiate(_missile, _gunPoint.transform.position, _gunPoint.transform.rotation).gameObject;
            //Get missle script and set parent
            Missile_Base missile = missileObj.GetComponent<Missile_Base>();
            missile.ParentShip = _shipWeapons;

            if (ShotAudioClip && ShotsPerburst % i < 1)
            {
                _shipWeapons.AudioSrc.PlayOneShot(ShotAudioClip);
            }
            yield return new WaitForSeconds(BurstShotDelay);
        }
        yield return null;
    }
}
