using UnityEngine;

public class PlayAudioFeedback : Feedback
{
    [SerializeField] AudioSource player;
    [SerializeField] AudioClip clip;

    private void Awake()
    {
        player = GetComponent<AudioSource>();
    }

    public override void Play(Vector3 playPos)
    {
        player?.PlayOneShot(clip);
    }
}