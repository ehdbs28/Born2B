using System;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
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
    
    private void SetBGM(AudioClip clip)
    {
        _audioSources[AudioType.BGM].Play(clip);
    }

    private void PlaySfx(AudioClip clip)
    {
        _audioSources[AudioType.SFX].Play(clip);
    }
}