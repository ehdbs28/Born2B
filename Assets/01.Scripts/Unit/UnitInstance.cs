//using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class UnitInstance : CellObjectInstance, IMovementable, IAttackable, IHitable
{
    [field: SerializeField] public List<int2> moveRole { get; set; }
    protected UnitFSMBase _unitFSMBase;
    protected UnitStatContainer _unitStatContainer;
    protected UnitWeaponController _weaponController;

    public float currebtHp { get; protected set; }

    public Vector2Int Position => transform.position.GetVectorInt();


    protected override void Awake()
    {
        base.Awake();

        _collider = GetComponent<Collider2D>();
        _unitStatContainer = GetComponent<UnitStatContainer>();
        _unitFSMBase = GetComponent<UnitFSMBase>();
        _weaponController = GetComponent<UnitWeaponController>();

    }



    public override void Init(CellObjectSO so)
    {
        
        base.Init(so);
        var casted = so as UnitDataSO;
        _unitStatContainer.Init(casted.stat);
        _weaponController.Init(casted.weaponItem, this);

        moveRole = casted.movementRole;

    }

    public bool Hit(CellObjectInstance attackObject, float damage, bool critical)
    {

        Debug.Log(currebtHp);
        if (attackObject is UnitInstance) return false;

        currebtHp -= critical ? 1 : 1;

        if(currebtHp <= 0)
        {

            Die();

        }

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
