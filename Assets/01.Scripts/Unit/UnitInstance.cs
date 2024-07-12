//using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UnitInstance : CellObjectInstance, IMovementable, IAttackable, IHitable
{
    [field: SerializeField] public List<int2> moveRole { get; set; }
    protected Collider2D _collider;
    protected UnitFSMBase _unitFSMBase;

    public Vector2Int Position => transform.position.GetVectorInt();


    protected override void Awake()
    {
        base.Awake();

        _collider = GetComponent<Collider2D>();
        _unitFSMBase = GetComponent<UnitFSMBase>();
    }

    protected virtual void Update()
    {

        _collider.enabled = TurnManager.Instance.GetTurnData<bool>(TurnDataType.IsMovementCell);

    }


    public bool Hit(CellObjectInstance attackObject, float damage, bool critical)
    {

        if (attackObject is UnitInstance) return true;

        Die();
        return true;
    }
    public void Attack()
    {

        _unitFSMBase.DoAttack();

    }

    private void Die()
    {
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

        CellObjectManager.Instance.RemoveCellObjectInstance(key);
        Destroy(gameObject);

        // 일단 다 true로 넘기기 후에 공격을 안받는 상황에서 false로 넘겨줘야 함
    }



    public Vector2 Move(List<Vector2> targetPositions, Action endCallback)
    {

        return _unitFSMBase.DoMove(targetPositions, endCallback);

    }

}
