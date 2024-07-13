using Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CellObjectManager : MonoSingleton<CellObjectManager>
{

    [SerializeField] private List<CellObjectByChapter> _prefabs;

    private Dictionary<StageObjectType, GameObject> _prefabContainer = new();
    private Dictionary<Guid, CellObjectSO> _objectContainer = new();
    private Dictionary<Guid, CellObjectInstance> _instanceContainer = new();
    private Dictionary<int2, CellObjectSO> _notMoveObjectContainer = new();
    private Collider2D _collider;

    public void Init()
    {
        
        foreach(var item in _prefabs)
        {

            _prefabContainer.Add(item.objType, item.obj);

        }

        _collider = GetComponent<Collider2D>();

    }

    protected virtual void Update()
    {

        if(_collider != null)
            _collider.enabled 
                = !TurnManager.Instance.GetTurnData<bool>(TurnDataType.IsPreview);

    }
    public void InitContainer()
    {

        foreach(var item in _instanceContainer.Values)
        {

            Destroy(item.gameObject);

        }

        _objectContainer.Clear();
        _instanceContainer.Clear();
        _notMoveObjectContainer.Clear();

    }

    public CellObjectSO GetCellObject(Guid id)
    {

        _objectContainer.TryGetValue(id, out CellObjectSO cellObject);

        if(cellObject != null)
        {

            return cellObject;

        }

        _instanceContainer.TryGetValue(id, out var ins);

        if(ins != null)
        {

            _objectContainer.TryGetValue(ins.dataKey, out cellObject);
            return cellObject;


        }

        return null;

    }

    public T GetCellObjectInstance<T>() where T : CellObjectInstance
    {

        foreach(var item in _instanceContainer.Values)
        {

            if(item is T)
            {

                return item as T;

            }

        }

        return null;

    }

    public CellObjectInstance GetCellObjectInstance(Guid id)
    {

        _instanceContainer.TryGetValue(id, out CellObjectInstance cellObjectInstance);

        return cellObjectInstance;

    }

    public CellObjectInstance GetCellObjectInstance(int2 pos)
    {

        _notMoveObjectContainer.TryGetValue(pos, out var value);

        if(value != null)
        {

            var obj = _instanceContainer.Values.First(x => x.dataKey == value.key);
            return obj;

        }

        return null;

    }

    public CellObjectInstance CreateCellObject(int2 pos, CellObjectSO data)
    {

        data = data is PlayerSelectSO ?
            (data as PlayerSelectSO).playerDatas[UnitSelectManager.Instance.selectedIdx] 
            : data.Clone() as CellObjectSO;
        data.position = pos;

        _objectContainer.Add(data.key, data);

        if(data.cellObjectType == CellObjectType.NotMoveAble)
        {

            _notMoveObjectContainer.Add(pos, data);

        }

        var obj = Instantiate(_prefabContainer[data.ObjectType]);

        if(obj.TryGetComponent<CellObjectInstance>(out var compo))
        {

            compo.Init(data);
            _instanceContainer.Add(compo.key, compo);

            return compo;

        }

        Destroy(obj);

        return null;

    }

    public CellObjectInstance CloneCellObjectInstance(Guid id)
    {

        if (!_instanceContainer.ContainsKey(id))
        {

            return null;

        }

        var obj = _instanceContainer[id].Clone() as CellObjectInstance;

        _instanceContainer.Add(obj.key, obj);
        return obj;

    }

    public void RemoveCellObjectInstance(Guid id)
    {

        if (_instanceContainer.ContainsKey(id))
        {

            _instanceContainer.Remove(id);

        }

    }

    public List<T> QueryCellObjectInstance<T>(Predicate<T> match) where T : CellObjectInstance
    {

        List<T> v = new List<T>();

        foreach(var item in _instanceContainer.Values)
        {

            var obj = item as T;

            if (obj == null) continue;

            if(match(obj))
            {

                v.Add(obj);

            }

        }

        return v;

    }

    public void MovementCellObject()
    {

        List<CellObjectInstance> moveObjContainer = new();

        foreach(var item in _instanceContainer.Values)
        {

            if(item is IMovementable)
            {
                var obj = item as IMovementable;
                List<Vector2> movePoss = new();
                Dictionary<Vector2, (int2 origin, int2 target)> container = new();

                foreach (var pos in obj.moveRole)
                {
                    
                    var targetPos = item.GetData().position + pos;
                    var oldPos = item.GetData().position;
                    var movePos = TryGetMovementPos(targetPos);

                    if (movePos != null)
                    {

                        movePoss.Add(movePos.Value);
                        container.Add(movePos.Value, (oldPos, targetPos));

                    }

                }

                if(movePoss.Count > 0)
                {

                    moveObjContainer.Add(obj as CellObjectInstance);
                    var vec = obj.Move(movePoss, () =>
                    {

                        moveObjContainer.Remove(obj as CellObjectInstance);

                    });

                    (var oldPos, var targetPos) = container[vec];
                    StageManager.Instance.Grid.SetUnitKey(oldPos, Guid.Empty);
                    StageManager.Instance.Grid.SetUnitKey(targetPos, item.key);
                    StageManager.Instance.Grid.ApplyCellParent(targetPos, item);
                    _objectContainer[item.dataKey].position = targetPos;

                }

            }


        }

        if (moveObjContainer.Count == 0)
        {
            
            TurnManager.Instance.EndCurrentTurn();
            return;

        }

        StartCoroutine(WaitAllObjectMove(moveObjContainer));

        IEnumerator WaitAllObjectMove(List<CellObjectInstance> inss)
        {

            yield return new WaitUntil(() => inss.Count == 0);
            TurnManager.Instance.EndCurrentTurn();

        }

    }

    public void AttackCellObject<T>(bool immediateEnd = false) where T : CellObjectInstance, IAttackable 
    {

        var list = QueryCellObjectInstance<T>(x => !x.isSkip);

        foreach (var item in list)
        {

            if (item is T)
            {
                
                var obj = item.GetComponent<T>();
                obj.Attack();

            }

        }

        if (immediateEnd)
        {

            TurnManager.Instance.EndCurrentTurn();

        }

    }

    private List<CellObjectInstance> GetHitInstance(List<int2> attackRole, int2 position)
    {

        var grid = StageManager.Instance.Grid.GetGrid();
        List<CellObjectInstance> hits = new();

        foreach(var item in attackRole)
        {
            var target = position + item;
            if (!grid.CheckOutBounce(target) && grid[target].unitKey != Guid.Empty)
            {

                var ins = _instanceContainer[grid[target].unitKey];
                hits.Add(ins);

            }

        }

        return hits;

    }

    public void DestroyCellObject(Guid id)
    {

        if(_instanceContainer.TryGetValue(id, out var obj))
        {

            StageManager.Instance.Grid.SetUnitKey(obj.GetData().position, Guid.Empty);
            _objectContainer.Remove(obj.dataKey);
            _instanceContainer.Remove(id);

            Destroy(obj.gameObject);

        }

    }

    private Vector2? TryGetMovementPos(int2 targetIdx)
    {

        var grid = StageManager.Instance.Grid.GetGrid();

        if (!grid.CheckOutBounce(targetIdx) && !_notMoveObjectContainer.ContainsKey(targetIdx))
        {

            if (grid[targetIdx].unitKey == Guid.Empty)
            {

                return StageManager.Instance.Grid.ConvertGridPoint(new Vector2(targetIdx.x, targetIdx.y));

            }

        }

        return null;

    }

    public void ChangePosition(Guid objectKey, int2 targetPos)
    {

        var ins = _instanceContainer[objectKey];

        if (!ins.isClone)
        {

            var oldPos = ins.GetData().position;
            StageManager.Instance.Grid.SetUnitKey(oldPos, Guid.Empty);

        }
        else
        {

            var idx = StageManager.Instance.Grid.FindCellIdxByUnit(objectKey);
            StageManager.Instance.Grid.SetCellUnitKey(idx, Guid.Empty);

        }

        StageManager.Instance.Grid.SetUnitKey(targetPos, ins.key);
        StageManager.Instance.Grid.ApplyCellParent(targetPos, ins);
        ins.GetData().position = targetPos;

    }

    public void DestroyCloneObjects(Guid key, Guid dataKey)
    {

        var datas = _instanceContainer.Values.Where(x => x.dataKey == dataKey && x.key != key).ToList();

        foreach(var item in datas)
        {
            int idx = StageManager.Instance.Grid.FindCellIdxByUnit(item.key);
            StageManager.Instance.Grid.SetCellUnitKey(idx, Guid.Empty);
            Destroy(item.gameObject);
            RemoveCellObjectInstance(item.key);

        }

    }

    internal void CheckHaveInstance(Guid unitKey)
    {

        if (!_instanceContainer.ContainsKey(unitKey))
        {

            var objs = FindObjectsOfType<CellObjectInstance>().ToList();

            var obj = objs.Find(x => x.key == unitKey);

            if(obj != null)
            {

                _instanceContainer.Add(obj.key, obj);

            }

        }

    }

    public List<T> GetCellObjectInstances<T>() where T : CellObjectInstance
    {

        var ls = _instanceContainer.Values.Where(x => x is T).Cast<T>().ToList();

        return ls;

    }

    [Serializable]
    public class CellObjectByChapter
    {

        public StageObjectType objType;
        public GameObject obj;

    }

}