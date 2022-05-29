using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UISoundManager : MonoBehaviour
{
    public AudioClip ButtonOnHighlightSound;
    public AudioClip ButtonOnPressedSound;

    private AudioSource a_source;
    private void Awake()
    {
        a_source = GetComponent<AudioSource>();
    }

    public void PlayOnHightlightedSound()
    {
        a_source.PlayOneShot(ButtonOnHighlightSound);
    }

    public void PlayOnPressedSound()
    {
        a_source.PlayOneShot(ButtonOnPressedSound);
    }
}
