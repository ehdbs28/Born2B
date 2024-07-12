using UnityEngine;

[CreateAssetMenu(menuName = "SO/Data/AudioData")]
public class AudioData : ScriptableObject
{
    public AudioClip clip;
    public AudioType type;
}