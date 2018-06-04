using UnityEngine;
using System.Collections;

public class AudioManager : MonoSingleton<AudioManager>
{
    private AudioSource _audioSource;
    private bool _isInit;

    public void Init()
    {
        _audioSource = Instance.gameObject.GetComponent<AudioSource>();

        if (_audioSource == null)
            _audioSource = Instance.gameObject.AddComponent<AudioSource>();

        _audioSource.playOnAwake = false;
        _audioSource.loop = true;
        _isInit = true;
    }

    public static void PlaySound(AudioClip clip)
    {
        Instance.PlaySoundCore(clip);
    }

    public void PlaySoundCore(AudioClip clip)
    {
        if (!_isInit)
            Init();

        _audioSource.PlayOneShot(clip);
    }

    public static void PlayMusic(AudioClip clip)
    {
        Instance.PlayMusicCore(clip);
    }

    private void PlayMusicCore(AudioClip clip)
    {
        if (!_isInit)
            Init();

        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public static void StopMusic()
    {
        Instance.StopMusicCore();
    }

    private void StopMusicCore()
    {
        if (!_isInit)
            Init();

        _audioSource.Stop();
    }
}