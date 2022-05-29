using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorDamager : MonoBehaviour, IDamagable
{
    [HideInInspector] public bool CanGetDestroyed;

    [Tooltip("Gameobject that plays the death effect")]
    [SerializeField] private ShipDeathEffect DeathEffect;

    [SerializeField] private GameObject DeathParticleEffect;
    [SerializeField] private AudioClip DeahtAudioClip;

   

    public void TakeDamage(ShipWeapons _source)
    {
        //now we have the abillity to add OnHitEffects that also destroy meteors
        if (CanGetDestroyed == false)
            return;

        if(DeathEffect)
        {
            DeathEffect = Instantiate(DeathEffect, transform.position, transform.rotation);
            DeathEffect.Play(DeathParticleEffect, DeahtAudioClip);
        }
        Destroy(transform.root.gameObject);
    }
}
