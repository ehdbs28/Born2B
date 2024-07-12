using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly Stack<PoolableMono> _pool;

    private readonly PoolableMono _prefab;
    private readonly Transform _parent;
    
    public Pool(PoolableMono prefab, Transform parent, int cnt)
    {
        _pool = new Stack<PoolableMono>();
        _prefab = prefab;
        _parent = parent;

        for (var i = 0; i < cnt; i++)
        {
            _pool.Push(CreateNewPrefab());
        }
    }

    private PoolableMono CreateNewPrefab()
    {
        var obj = Object.Instantiate(_prefab, _parent);
        obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");
        obj.gameObject.SetActive(false);
        return obj;
    }

    public PoolableMono Pop()
    {
        var obj = _pool.Count <= 0 ? CreateNewPrefab() : _pool.Pop();
        obj.gameObject.SetActive(true);
        obj.state = PoolingState.PoolOut;
        obj.OnPop();
        return obj;
    }

    public void Push(PoolableMono obj)
    {
        if (obj.state == PoolingState.InPool)
        {
            return;
        }

        obj.OnPush();
        obj.state = PoolingState.InPool;
        obj.transform.SetParent(_parent);
        obj.gameObject.SetActive(false);
        _pool.Push(obj);
    }
}