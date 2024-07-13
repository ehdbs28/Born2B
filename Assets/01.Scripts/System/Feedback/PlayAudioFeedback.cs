using UnityEngine;

public class PlayAudioFeedback : Feedback
{
    [SerializeField] AudioSource player;
    [SerializeField] AudioLibrarySO library;
    [SerializeField] string key;

    private void Awake()
    {
        player = GetComponent<AudioSource>();
    }

    public override void Play(Vector3 playPos)
    {
        player?.PlayOneShot(library[key]);
    }
}