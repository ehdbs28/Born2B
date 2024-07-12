using System;
using UnityEngine;

public partial class PlayerInstance : CellObjectInstance, IHitable
{

    private bool _areadyDestroyed;
    protected Collider2D _collider;

    public Vector2Int Position => transform.position.GetVectorInt();

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<Collider2D>();

        InitPlayerComponents();
    }

    protected virtual void Update()
    {

        _collider.enabled = TurnManager.Instance.GetTurnData<bool>(TurnDataType.IsMovementCell);

    }

    protected virtual void OnDestroy()
    {
        ReleasePlayerComponents();   
    }

    public bool Hit(CellObjectInstance attackObject, float damage, bool critical)
    {
        PlayerHealthComponent health = GetPlayerComponent<PlayerHealthComponent>();
        if(health.CurrentHp <= 0)
            return false;

        health.ReduceHp(-1);
        EventManager.Instance.PublishEvent(EventType.OnPlayerDamaged);

        if(health.CurrentHp <= 0)
            Die();

        return true;
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

                var idx = StageManager.Instance.FindGridIdxByUnit(key);
                StageManager.Instance.SetUnitKey(idx, Guid.Empty);

            }
            else
            {

                var idx = StageManager.Instance.FindCellIdxByUnit(key);
                StageManager.Instance.SetCellUnitKey(idx, Guid.Empty);

            }

            _areadyDestroyed = true;
            EventManager.Instance.PublishEvent(EventType.OnPlayerDead);

        }
    }

}
