using System;
using UnityEngine;

public class CellObjectInstance : MonoBehaviour, ICloneable
{
    
    public Guid key { get; set; }
    public Guid dataKey { get; set; }
    public bool isClone { get; set; }
    public bool isSkip { get; set; }

    protected virtual void Awake()
    {
        EventManager.Instance.RegisterEvent(EventType.OnTurnEnded, HandleTurnEnded);
        EventManager.Instance.RegisterEvent(EventType.OnTurnChanged, HandleTurnChanged);
    }

    public CellObjectSO GetData()
    {

        return CellObjectManager.Instance.GetCellObject(dataKey);

    }

    private void HandleTurnEnded(object[] args) => OnTurnEnded();
    private void HandleTurnChanged(object[] args) => OnTurnChanged((TurnType)args[0], (TurnType)args[1]);

    protected virtual void OnTurnEnded()
    {
        isSkip = false;
    }

    protected virtual void OnTurnChanged(TurnType prevTurn, TurnType nextTurn) { }

    public virtual object Clone()
    {

        var obj = Instantiate(this);
        obj.key = Guid.NewGuid();
        obj.dataKey =dataKey;
        obj.isClone = true;

        return obj;

    }
}
