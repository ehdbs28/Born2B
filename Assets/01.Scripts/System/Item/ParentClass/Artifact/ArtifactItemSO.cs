public abstract class ArtifactItemSO : ItemSO
{
    protected abstract ArtifactType ArtifactType { get; }
    protected virtual EventType CallingEventType => 0;

    protected IItemHandler OwnerHandler;

    public override bool Execute(IItemHandler handler)
    {
        OwnerHandler = handler;
        
        // 체력 회복 같이 먹자마자 바로 적용되는거
        if (ArtifactType == ArtifactType.UseImmediately)
        {
            UseArtifact();
        }
        else if (ArtifactType == ArtifactType.CallByEvent)
        {
            RegisterEvent();
        }
        return true;
    }

    public override bool Unexecute(IItemHandler handler)
    {
        if (ArtifactType == ArtifactType.CallByEvent)
        {
            UnRegisterEvent();
        }
        OwnerHandler = null;
        return true;
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