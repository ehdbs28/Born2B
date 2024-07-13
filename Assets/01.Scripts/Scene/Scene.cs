public abstract class Scene : PoolableMono
{
    public abstract SceneType Type { get; }

    public virtual void EnterScene()
    {
        
    }

    public virtual void ExitScene()
    {
        PoolManager.Instance.Push(this);
    }
}