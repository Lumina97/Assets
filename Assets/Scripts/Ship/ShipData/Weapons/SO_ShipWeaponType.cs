using System.Collections;
using UnityEngine;

public abstract class SO_ShipWeaponType : ScriptableObject
{
    [SerializeField] protected float ShotDelay;
    public virtual float GetShotDelay
    {
        get { return ShotDelay; }
    }
    public AudioClip ShotAudioClip;

    public abstract IEnumerator Fire(GameObject _gunPoint, Missile_Base _missile, ShipWeapons _shipWeapons);
}