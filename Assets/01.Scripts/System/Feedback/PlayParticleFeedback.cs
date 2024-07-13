using System.Collections.Generic;
using UnityEngine;

public class PlayParticleFeedback : Feedback
{
    [SerializeField] float delay = 0.6f;
    private List<ParticleSystem> particles = null;

    private void Awake()
    {
        particles = new List<ParticleSystem>();
        transform.GetComponentsInChildren<ParticleSystem>(particles);
    }

    public override void Play(Vector3 playPos)
    {
        StartCoroutine(this.DelayCoroutine(delay, () => particles.ForEach(i => i.Play())));
    }
}