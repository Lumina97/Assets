using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PowerUp : PoolingItemBase
{
    public delegate void OnPowerUpUsedDelegate(PowerUp _powerUp);
    public OnPowerUpUsedDelegate OnPowerUpUsed;

    private SO_Powerup _powerUp;

    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSrc;
    private bool _hasBeenUsed;

    private void Awake()
    {
        OnActiveStateChanged += ActiveState;
    }

    private void ActiveState(bool _active)
    {
        if (_spriteRenderer)
        {
            _spriteRenderer.enabled = _active;
        }
        if (_audioSrc)
        {
            _audioSrc.enabled = _active;
        }
    }

    public void SetPowerUP(SO_Powerup _powerUp)
    {
        //set powerup
        this._powerUp = _powerUp;

        //get sprite rendere and set sprite
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer && this._powerUp && this._powerUp.Visuals)
        {
            _spriteRenderer.sprite = this._powerUp.Visuals;
        }
        //get audiosource
        _audioSrc = GetComponent<AudioSource>();
    }

    public void UsePowerUp(GameObject _ship)
    {
        //check if we have a powerup and if it has been used or not
        if (_powerUp && _hasBeenUsed == false)
        {
            if (_powerUp.CanUsePowerUpOnShip(_ship) == false)
                return;
            //then do what ever the powerup does
            StartCoroutine(_powerUp.PowerUP(_ship));

            //play a audioclip if we have one
            if (_audioSrc && _powerUp.UseSound)
            {
                _audioSrc.PlayOneShot(_powerUp.UseSound);
            }

            //play particle effect if we have one
            if (_powerUp.UseParticleEffect)
            {
                Destroy(Instantiate(_powerUp.UseParticleEffect, transform.position, transform.rotation), 5f);
            }

            //set to destroy this object after we the powerup has ended
            //added 1 second just to be sure
            //since what ever object calls a Coroutine then owns that coroutine
            //and if we destroy it before the powerup has worn off 
            //the player has the powerup for ever since the coroutine gets stopped 
            //if the owning object gets destroyed
            Destroy(gameObject, _powerUp.EffectDuration + 1);

            //set has been used 
            //and clear the sprite 
            //so we cannot interact or see it in the world
            //while we wait for the powerupeffect to run out
            _hasBeenUsed = true;
            _spriteRenderer.sprite = null;
            if (OnPowerUpUsed != null)
                OnPowerUpUsed(this);
        }
    }
}
