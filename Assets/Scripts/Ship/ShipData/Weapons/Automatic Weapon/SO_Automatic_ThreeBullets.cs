using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship/Weapon/Automatic/Tree Missiles")]
public class SO_Automatic_ThreeBullets : SO_ShipWeaponType
{
    public float DistanceToEachShot = 0.5f;

    public override IEnumerator Fire(GameObject _gunPoint, Missile_Base _missile, ShipWeapons _shipWeapons)
    {
        //Spawn Missle
        for (int i = 0; i < 3; i++)
        {
            Vector3 gunpos =  _gunPoint.transform.position;    

            float f_offset = -DistanceToEachShot + (i * DistanceToEachShot);
            Vector3 offsetVec = new Vector3(gunpos.x + f_offset, gunpos.y);


            Vector3 _position = gunpos + offsetVec;

            //Debug.Log(up);

            GameObject missileObj = Instantiate(_missile, _position, _gunPoint.transform.rotation).gameObject;

            //Get missle script and set parent
            Missile_Base missile = missileObj.GetComponent<Missile_Base>();
            missile.ParentShip = _shipWeapons;
        }

        if (ShotAudioClip)
        {
            _shipWeapons.AudioSrc.PlayOneShot(ShotAudioClip);
        }
        yield return null;
    }
}