using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public void PlayAudio(AudioData data)
    {
        AudioManager.Instance.PlayAudio(data);
    }
}