using UnityEngine;

public abstract class Feedback : ExtendedMono
{
    public abstract void Play(Vector3 playPos);
    public virtual void Stop() { }
}