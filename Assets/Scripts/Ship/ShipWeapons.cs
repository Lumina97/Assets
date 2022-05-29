using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShipWeapons : MonoBehaviour
{
    [SerializeField] private GameObject Gunpoint;
    [SerializeField] private SO_ShipWeaponType DefaultPrimaryWeaponType;
    [SerializeField] private SO_MissileOnHitEffect DefaultOnHitEffect;
    [SerializeField] private Missile_Base DefaultMissile;

    [HideInInspector] public AudioSource AudioSrc;

    private bool _bShotDelayRunning;

    private SO_ShipWeaponType PrimaryWeapon;
    //used to track if a powerup has already changed the weapontype
    [HideInInspector]  public bool _hasWeaponTypeUpgrade;
    /// <summary>
    /// sets a new weapontype
    /// if null then the default weapontype will be used
    /// </summary>
    public SO_ShipWeaponType CurrentPrimaryWeapon
    {
        get { return PrimaryWeapon; }
        set
        {
            if (value != null && _hasWeaponTypeUpgrade == false)
            {
                _hasWeaponTypeUpgrade = true;
                PrimaryWeapon = value;
            }
            else if( value == null )
            {
                _hasWeaponTypeUpgrade = false;
                PrimaryWeapon = DefaultPrimaryWeaponType;
            }
        }
    }

    private Missile_Base _currentMissile;
    //used to track if a powerup has already changed the missiletype
    [HideInInspector]  public bool _hasMissileUpgrade;
    /// <summary>
    /// sets a new missiletype
    /// if null then the default missiletype will be used
    /// </summary>
    public Missile_Base CurrentMissile
    {
        get { return _currentMissile; }
        set
        {
            if (value != null && _hasMissileUpgrade == false)
            {
                _hasMissileUpgrade = true;
                _currentMissile = value;
            }
            else if( value == null )
            {
                _hasMissileUpgrade = false;
                _currentMissile = DefaultMissile;
            }
        }
    }


    private SO_MissileOnHitEffect _currentMissleOnHitEffect;
    [HideInInspector] public bool _hasMissileOnHitEffect;
    /// <summary>
    /// sets a new missileOnHitEffect
    /// if null then the default onHitEffect will be used
    /// </summary>
    public SO_MissileOnHitEffect CurrentMissileOnHit
    {
        get { return _currentMissleOnHitEffect; }
        set
        {
            if (value != null && _hasMissileOnHitEffect == false)
            {
                _hasMissileOnHitEffect = true;
                _currentMissleOnHitEffect = value;
            }
            else if (value == null)
            {
                _hasMissileOnHitEffect = false;
                _currentMissleOnHitEffect = DefaultOnHitEffect;
            }
        }
    }

    private void Awake()
    {
        AudioSrc = GetComponent<AudioSource>();
        CurrentPrimaryWeapon = null;
    }

    public void FireWeapon()
    {
        if (Gunpoint && PrimaryWeapon && GetCanFireWeapon())
        {
            StartCoroutine(PrimaryWeapon.Fire(Gunpoint, DefaultMissile, this));
            StartCoroutine(ShotDelay(PrimaryWeapon.GetShotDelay));
        }
    }

    /// <summary>
    /// Manages shot delay / firerate
    /// </summary>
    /// <param name="_time">lenght of the delay</param>
    /// <returns></returns>
    private IEnumerator ShotDelay(float _time)
    {
        _bShotDelayRunning = true;
        yield return new WaitForSeconds(_time);
        _bShotDelayRunning = false;
    }

    private bool GetCanFireWeapon()
    {
        //Change as needed
        return !_bShotDelayRunning;
    }
}