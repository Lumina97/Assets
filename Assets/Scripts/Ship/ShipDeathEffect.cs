using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShipDeathEffect : MonoBehaviour
{
    public void Play(GameObject _particle, AudioClip _audioClip)
    {
        if (_audioClip != null)
            GetComponent<AudioSource>().PlayOneShot(_audioClip);
        if (_particle != null)
            Instantiate(_particle,transform.position,transform.rotation);

        Destroy(gameObject, 5.0f);
    }
}
