using System.Collections.Generic;
using System.Linq;
using Singleton;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    private readonly Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    [SerializeField] private List<PoolingList> _poolingLists;
    public List<PoolingList> PoolingLists => _poolingLists;

    public void Awake()
    {
        foreach (var pair in _poolingLists.SelectMany(poolingList => poolingList.poolableItems))
        {
            CreatePool(pair.prefab, transform, pair.cnt);
        }
    }

    private void CreatePool(PoolableMono prefab, Transform parent, int cnt)
    {
        if (_pools.ContainsKey(prefab.name))
        {
            return;
        }
        
        _pools.Add(prefab.name, new Pool(prefab, parent, cnt));
    }

    public PoolableMono Pop(PoolingItemType key)
    {
        return Pop(key.ToString());
    }

    public PoolableMono Pop(string key)
    {
        if (_pools.TryGetValue(key, out var pool))
        {
            var obj = pool.Pop();
            if (SceneControlManager.Instance.CurrentScene != null)
            {
                SceneControlManager.Instance.CurrentScene.AddObject(obj);
            }
            return obj;
        }
        
        Debug.LogError($"[PoolManager] Doesn't exist key on pools : [{key}]");
        return null;
    }

    public void Push(PoolableMono obj)
    {
        _pools[obj.name].Push(obj);
    }
}