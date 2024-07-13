using System.Collections.Generic;

public abstract class Scene : PoolableMono
{
    public abstract SceneType Type { get; }
    private List<PoolableMono> _poolingObjects = new List<PoolableMono>();

    public virtual void EnterScene()
    {
        _poolingObjects.Clear();
    }

    public virtual void ExitScene()
    {
        PoolManager.Instance.Push(this);
    }

    public void AddObject(PoolableMono obj)
    {
        if (obj == this || obj is TransitionScreen)
        {
            return;
        }
        
        _poolingObjects.Add(obj);
    }

    public override void OnPush()
    {
        foreach (var poolingObject in _poolingObjects)
        {
            PoolManager.Instance.Push(poolingObject);
        }
    }
}