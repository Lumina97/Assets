using System.Collections;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(ShipWeapons _source);
}

[RequireComponent(typeof(AudioSource))]
public class ShipHealth : MonoBehaviour, IDamagable
{
    public bool CanDie = true;
    [Tooltip("The gameobject that is spawned on death to play the partice and the audio.")]
    public ShipDeathEffect DeathEffectPrefab;
    public GameObject DeathParticle;
    public AudioClip DeathAudioClip;
    public AudioClip LowHealthClip;

    [Space]
    [SerializeField]
    private float InvincibillityTimeAfterReceivedDamage;
    private bool _canTakeDamage = true;
    public int MaxLives = 3;
    private int _currentLives;
    public int CurrentLives
    {
        get { return _currentLives; }
        set
        {
            //check if we currently have more health then what has been passed it
            // aka we take damage     
            bool tookDamage = _currentLives > value;

            //set currentlives but clamp it to 0 and maxlives
            _currentLives = value;
            _currentLives = Mathf.Clamp(_currentLives, 0, MaxLives);

            //stop low health coroutine to stop playing the sound
            if (tookDamage == false)
            {
                StopCoroutine("PlayLowHealthSound");
            }

            //call event if it has subscribers
            if (OnShipHealthChanged != null)
            {
                OnShipHealthChanged(_currentLives, tookDamage);
            }
        }
    }

    public delegate void OnShipHealthChangedDelegate(int _currentlives, bool _tookDamage = true);
    public OnShipHealthChangedDelegate OnShipHealthChanged;

    public delegate void OnShipDeathDelegate(ShipHealth _shipHealth);
    public OnShipDeathDelegate OnShipDeath;

    private AudioSource _audioSource;
    void Awake()
    {
        //get references and set lives 
        _audioSource = GetComponent<AudioSource>();
        _currentLives = MaxLives;
    }

    /// <summary>
    /// Damages the ship.
    /// Checks if the damage source has the same tag
    /// to prevent AI to destroy each other
    /// </summary>
    /// <param name="_source"></param>
    public void TakeDamage(ShipWeapons _source = null)
    {
        if (_canTakeDamage == false)
            return;
        //check if we are running in the editor
        //so we do not accedentally have an invincible ship
        //in a build
        if (Application.isEditor)
        {
            //check we want a invincible ship..
            if (CanDie == false)
                return;
        }

        if (_source != null)
        {
            //if the tag is the same as the tag of the source who fired the shot
            //return. This stops Ai from shooting each other
            if (gameObject.tag == _source.gameObject.tag)
                return;
        }

        //check if the game is paused - if so we return since we don't want to take damage from still moving projectiles
        if (GameManager.Instance && GameManager.Instance.GameState == EGameState.paused)
            return;

        //check if deahtparticle or Audioclip is no null
        if (DeathEffectPrefab)
        {
            Instantiate(DeathEffectPrefab, transform.position, transform.rotation).GetComponent<ShipDeathEffect>().Play(DeathParticle, DeathAudioClip);
        }

        //check if health is lower or equal to 0 when reducing it by one
        //if so we die
        CurrentLives--;

        //check for low health and if so start playing audioclip
        if(_currentLives == 1)
        {
            StartCoroutine("PlayLowHealthSound");
        }

        //Restart Invincibility coroutine
        StopCoroutine("SetInvincibillityTimer");
        StartCoroutine("SetInvincibillityTimer");
        if (CurrentLives == 0)
            Death();
    }

    /// <summary>
    /// Checks if Audiosource and clip is available then plays a low health sound
    /// repeats automaticly as long as the ship is on low health.
    /// </summary>
    private IEnumerator PlayLowHealthSound()
    {
        //check if we even have a audioclip for low health
        if(LowHealthClip == null)
        {
            yield break;
        }
        //check if we have an audiosource to play the clip
        if(_audioSource == null)
        {
            yield break;
        }

        //play clip
        _audioSource.PlayOneShot(LowHealthClip);
        //wait for the duration of it 
        yield return new WaitForSeconds(LowHealthClip.length);

        //if we are still low on health we restart the coroutine
        if(_currentLives == 1)
        {
            StartCoroutine("PlayLowHealthSound");
        }
    }

    /// <summary>
    /// makes the ship invincible for a short time so the player can't  die within half a second
    /// </summary>
    private IEnumerator SetInvincibillityTimer()
    {
        _canTakeDamage = false;
        yield return new WaitForSeconds(InvincibillityTimeAfterReceivedDamage);
        _canTakeDamage = true;
    }

    /// <summary>
    /// kills the ship and calls an event
    /// </summary>
    private void Death()
    {
        //check if we have subscribed methods
        //call callback
        if (OnShipDeath != null)
        {
            OnShipDeath(this);
        }

        //destory the gameobject
        Destroy(transform.root.gameObject);
    }
}