using System.Collections;
using UnityEngine;

public abstract class SO_Powerup : ScriptableObject
{
    [Header("Base PowerUp")]
    public Sprite Visuals;
    public AudioClip UseSound;
    public GameObject UseParticleEffect;
    public float EffectDuration;

    protected GameObject _ship;

    public abstract bool CanUsePowerUpOnShip(GameObject _ship);
    public IEnumerator PowerUP(GameObject _ship)
    {
        this._ship = _ship;
        if(_ship == null)
        {
            Debug.Log("The ship that has been passed in is invalid. aka null. SO_Powerup.cs");
            yield break;
        }
        StartEffect();
        yield return new WaitForSeconds(EffectDuration);
        StopEffect();
    }

    protected abstract void StartEffect();
    protected abstract void StopEffect();
}
