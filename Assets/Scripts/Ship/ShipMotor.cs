using System.Collections;
using UnityEngine;

/// <summary>
/// Must be placed on the base object of the ship
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class ShipMotor : MonoBehaviour
{
    public enum ETurnDirection
    {
        none = 0,
        left = 1,
        right = -1
    }

    #region Movement variables

    [Space]
    public SO_ShipMovementData MovementData;
    public ParticleSystem BoosterParticleSystem;
    [Tooltip("The min velocity to explode on impact")]
    public float ExplosionVelocityThreshhold = 14f;

    private float _lastVelocity;

    private Vector2 movementVec;
    private float _forwardInput;

    //Booster
    private bool _rechargeBooster;
    private float _currentBoosterReserves = 1.0f;
    public float GetCurrentBoosterReserves
    {
        get { return _currentBoosterReserves; }
    }
    #endregion Movement variables

    #region Turn variables

    [Space]
    public SO_ShipTurnData TurnData;

    private ETurnDirection _turnInput;

    #endregion Turn variables

    #region Components

    private Rigidbody2D r_Body;
    private AudioSource a_Source;

    #endregion Components

    public delegate void OnFatalVelocityChangeDelegate();
    public OnFatalVelocityChangeDelegate OnFatalVelocityChange;

    // Use this for initialization
    private void Awake()
    {
        r_Body = GetComponent<Rigidbody2D>();
        a_Source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Turn();
        if (_forwardInput > 0f)
            UseBooster();

        if (_lastVelocity >= ExplosionVelocityThreshhold && Vector2.SqrMagnitude(r_Body.velocity) <= (_lastVelocity / 1.5f))
        {
            if (OnFatalVelocityChange != null)
                OnFatalVelocityChange();
        }

        if (_rechargeBooster && _currentBoosterReserves < 1)
        {
            _currentBoosterReserves = Mathf.Lerp(0, 1, _currentBoosterReserves + MovementData.BoosterRechargeSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetMovement(float _forwardMove, ETurnDirection _turnDirection, bool _disableMovement = false)
    {
        if (_disableMovement)
        {
            r_Body.Sleep();
        }
        else if (r_Body.IsSleeping())
            r_Body.WakeUp();

        if (_currentBoosterReserves > 0)
            _forwardInput = _forwardMove;
        else
            _forwardInput = 0;

        _turnInput = _turnDirection;
    }

    private void UseBooster()
    {
        StopCoroutine("RechargeBooster");
        _rechargeBooster = false;
        if (a_Source && MovementData.AccelerationAudioClip)
        {
            a_Source.clip = MovementData.AccelerationAudioClip;
            if (a_Source.isPlaying == false)
                a_Source.Play();
        }
        if (BoosterParticleSystem)
            BoosterParticleSystem.Play();
        _currentBoosterReserves = Mathf.Clamp(_currentBoosterReserves - MovementData.BoosterDepletionSpeed * Time.deltaTime, 0, 1);

        StartCoroutine("RechargeBooster");
    }

    private IEnumerator RechargeBooster()
    {
        yield return new WaitForSeconds(MovementData.BoosterRechargeDelay);

        if (a_Source.isPlaying)
            a_Source.Pause();

        if (BoosterParticleSystem)
            BoosterParticleSystem.Stop();
        _rechargeBooster = true;
    }

    private void Move()
    {
        //catch if we have no movement data
        if (MovementData == null)
        {
            //.. log it..
            Debug.LogError("No MovementData has been assigned to this ship! - " + gameObject.name);
            // and leave function
            return;
        }
        _lastVelocity = Vector2.SqrMagnitude(r_Body.velocity);

        movementVec = (transform.rotation * Vector2.up) * _forwardInput * MovementData.AccellerationSpeed;
        r_Body.AddForce(movementVec);
    }

    private void Turn()
    {
        //catch if we have no movement data
        if (TurnData == null)
        {
            //.. log it..
            Debug.LogError("No TurnData has been assigned to this ship! - " + gameObject.name);
            // and leave function
            return;
        }

        //check if turninput is not none..
        if (_turnInput != ETurnDirection.none)
        {
            //then get new vector based on the turn direction..
            float rotation = Time.deltaTime * (float)_turnInput * TurnData.TurnAcceleration + transform.eulerAngles.z;
            //turn the ship
            transform.eulerAngles = new Vector3(0, 0, rotation);
        }
    }
}