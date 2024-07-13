using System;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerInstance : CellObjectInstance, IHitable
{
    private bool _areadyDestroyed;

    public Vector2Int Position => transform.position.GetVectorInt();

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<Collider2D>();

        //InitPlayerComponents();
    }

    public override void Init(CellObjectSO so)
    {
        base.Init(so);

        InitPlayerComponents();

        var castSo = so as UnitDataSO;
        castSo.health = Health;
        castSo.statusController = GetComponent<StatusController>();   
    }

    protected virtual void OnDestroy()
    {
        ReleasePlayerComponents();
    }

    public override object Clone()
    {
        PlayerInstance clone = base.Clone() as PlayerInstance;
        playerModuleList.ForEach(i => i.OnClone(clone));
        playerComponentList.ForEach(i => i.OnClone(clone));

        return clone;
    }

    public void Hit(CellObjectInstance attackObject, float damage, bool critical, Action<CellObjectInstance> callBack)
    {
        PlayerHealthComponent health = GetPlayerComponent<PlayerHealthComponent>();

        health.ReduceHp(1);
        EventManager.Instance.PublishEvent(EventType.OnPlayerDamaged);

        if(health.CurrentHp <= 0)
            Die();

        callBack?.Invoke(this);
    }

    private void Die()
    {

        if (!_areadyDestroyed)
        {

            Destroy(gameObject);
            CellObjectManager.Instance.DestroyCloneObjects(key, dataKey);
            CellObjectManager.Instance.RemoveCellObjectInstance(key);

            if (!isClone)
            {

                var idx = StageManager.Instance.Grid.FindGridIdxByUnit(key);
                StageManager.Instance.Grid.SetUnitKey(idx, Guid.Empty);

            }
            else
            {

                var idx = StageManager.Instance.Grid.FindCellIdxByUnit(key);
                StageManager.Instance.Grid.SetCellUnitKey(idx, Guid.Empty);

            }

            _areadyDestroyed = true;
            EventManager.Instance.PublishEvent(EventType.OnPlayerDead);

        }
    }

}
