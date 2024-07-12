using UnityEngine;

public class AudioSourceController : ExtendedMono
{
    [SerializeField] private AudioType _sourceType;
    public AudioType SourceType => _sourceType;

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _source.loop = _sourceType == AudioType.BGM;
        _source.playOnAwake = false;
    }
    
    public void Play(AudioClip clip)
    {
        if (_sourceType == AudioType.BGM)
        {
            _source.clip = clip;
            _source.Play();
        }
        else
        {
            _source.PlayOneShot(clip);
        }
    }

    public void Stop()
    {
        _source.Stop();
        _source.clip = null;
    }
}