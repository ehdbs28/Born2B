public abstract class AttributedCallByEventItemSO : ArtifactItemSO
{
    protected override ArtifactType artifactType => ArtifactType.Attributed;

    public override void Execute(IItemHandler handler)
    {
        OwnerHandler = handler;
        RegisterEvent();
    }

    public override void Unexecute(IItemHandler handler)
    {
        UnRegisterEvent();
        OwnerHandler = null;
    }
}
