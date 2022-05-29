using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip[] MusicToPlay;

    private float _currentClipLenght;
    private AudioSource _audioSource;

    bool _isPlaying;
    bool _isPaused;

    public static MusicManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _audioSource = GetComponent<AudioSource>();
    }

    public void StartMusicPlay()
    {
        AudioClip cliptoplay = GetRandomMusicClip();
        _currentClipLenght = cliptoplay.length;
        _audioSource.clip = cliptoplay;
        _audioSource.Play();
        _isPlaying = true;
        StartCoroutine("InsertNextSongToPlay");
    }

    private IEnumerator InsertNextSongToPlay()
    {
        yield return new WaitForSeconds(_currentClipLenght);
        StartMusicPlay();
    }

    public void PauseMusicPlay()
    {
        if (_isPlaying)
        {
            _isPaused = true;
            _audioSource.Pause();
        }
    }

    public void ResumeMusicPlay()
    {
        if (_isPaused)
        {
            _isPaused = false;
            _audioSource.UnPause();
        }
    }

    public void SkipSong()
    {
        StopCoroutine("InsertNextSongToPlay");
        StartMusicPlay();
    }

    private AudioClip GetRandomMusicClip()
    {
        int num = Random.Range(1, MusicToPlay.Length);
        AudioClip clip = MusicToPlay[num];

        //Move it around so we dont hear the same track twice in a row
        MusicToPlay[num] = MusicToPlay[0];
        MusicToPlay[0] = clip;
        return clip;
    }

    public void PlaySpecialClip(AudioClip _clipToPlay, bool _returnToMusic = true)
    {
        _audioSource.clip = _clipToPlay;
        _currentClipLenght = _clipToPlay.length;
        if (_returnToMusic)
        {
            StartCoroutine("InsertNextSongToPlay");
        }
    }
}
