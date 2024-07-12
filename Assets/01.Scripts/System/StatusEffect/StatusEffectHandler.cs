public abstract class StatusEffectHandler
{
    protected CellObjectInstance owner = null;

    public virtual void Init(CellObjectInstance owner)
    {
        this.owner = owner;
    }

	public virtual void HandleBegin() {}
	public virtual void HandleUpdate() 
    {
        UnityEngine.Debug.Log($"{GetType()}Handler Updated");
    }
	public virtual void HandleEnd() {}
}
