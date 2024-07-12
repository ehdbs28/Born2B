using System;
using UnityEngine;

public class CellObjectInstance : MonoBehaviour, ICloneable
{

    protected Animator _animator;
    protected SpriteRenderer _renderer;
    protected Collider2D _collider;

    public Guid key { get; set; }
    public Guid dataKey { get; set; }
    public bool isClone { get; set; }
    public bool isSkip { get; set; }

    protected virtual void Awake()
    {
        EventManager.Instance.RegisterEvent(EventType.OnTurnEnded, HandleTurnEnded);
        EventManager.Instance.RegisterEvent(EventType.OnTurnChanged, HandleTurnChanged);
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {

        _collider.enabled = !TurnManager.Instance.GetTurnData<bool>(TurnDataType.IsPreview);

    }

    public virtual void Init(CellObjectSO so)
    {

        key = Guid.NewGuid();
        dataKey = so.key;
        if(_animator != null)
            _animator.runtimeAnimatorController = so.animator;

        if(_renderer != null)
            _renderer.sprite = so.sprite;

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
        obj.dataKey = dataKey;
        obj.isClone = true;
        obj.Init(GetData());

        return obj;

    }
}
