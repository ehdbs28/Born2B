using UnityEngine;

public class PlayAudioFeedback : Feedback
{
    [SerializeField] AudioData audioData;

    public override void Play(Vector3 playPos)
    {
        AudioManager.Instance.PlayAudio(audioData);
    }
}