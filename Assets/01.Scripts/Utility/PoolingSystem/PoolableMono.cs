using UnityEngine;

public abstract class PoolableMono : ExtendedMono
{
    [HideInInspector] public PoolingState state;

    public abstract void OnPop();
    public abstract void OnPush();
}