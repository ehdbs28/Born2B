using System;

public abstract class ArtifactItemSO : ItemSO
{
    protected abstract ArtifactType artifactType { get; }
    public ArtifactType ArtifactType => artifactType;

    protected virtual EventType CallingEventType => 0;
    public override Type ItemType => typeof(ArtifactItemSO);

    protected IItemHandler OwnerHandler;

    public override void Execute(IItemHandler handler)
    {
        OwnerHandler = handler;
        
        // 체력 회복 같이 먹자마자 바로 적용되는거
        if (artifactType == ArtifactType.UseImmediately)
        {
            UseArtifact();
        }
        else if (artifactType == ArtifactType.CallByEvent)
        {
            RegisterEvent();
        }
    }

    public override void Unexecute(IItemHandler handler)
    {
        if (artifactType == ArtifactType.CallByEvent)
        {
            UnRegisterEvent();
        }
        OwnerHandler = null;
    }

    public abstract void UseArtifact(params object[] args);

    private void RegisterEvent()
    {
        EventManager.Instance.RegisterEvent(CallingEventType, UseArtifact);
    }

    private void UnRegisterEvent()
    {
        EventManager.Instance.UnRegisterEvent(CallingEventType, UseArtifact);
    }
}