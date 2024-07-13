using System.Collections.Generic;
using Singleton;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioMixer audioMixer;
    private Dictionary<AudioType, AudioSourceController> _audioSources;

    private void Awake()
    {
        _audioSources = new Dictionary<AudioType, AudioSourceController>();

        var audioSourceList = new List<AudioSourceController>();
        GetComponentsInChildren<AudioSourceController>(audioSourceList);

        foreach (var sourceController in audioSourceList)
        {
            _audioSources.Add(sourceController.SourceType, sourceController);
        }
    }

    public void PlayAudio(AudioData data)
    {
        if (data.type == AudioType.BGM)
        {
            SetBGM(data.clip);
        }
        else if (data.type == AudioType.SFX)
        {
            PlaySfx(data.clip);
        }
    }

    public void SetVolume(AudioGroupType groupType, float volume)
    {
        audioMixer.SetFloat(groupType.ToString(), volume * 100f - 80f);
    }

    private void SetBGM(AudioClip clip)
    {
        _audioSources[AudioType.BGM].Play(clip);
    }

    private void PlaySfx(AudioClip clip)
    {
        _audioSources[AudioType.SFX].Play(clip);
    }
}