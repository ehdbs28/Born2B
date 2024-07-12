//using DG.Tweening;
using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DefaultUnitAttackState : UnitFSMStateBase
{

    protected Weapon _unitWeapon;

    public DefaultUnitAttackState(FSM_Controller<UnitStateType> controller) : base(controller)
    {

        _unitWeapon = controller.GetComponent<Weapon>();

    }

    protected override void EnterState()
    {

        var vec = transform.position.GetVectorInt();
        var player = CellObjectManager.Instance.GetCellObjectInstance<PlayerInstance>();
        var dir = player.transform.position - transform.position;
        var ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _unitWeapon.RotateAttackRange(ang);
        AttackParams attackParams = new AttackParams(_stats[StatType.Attack], _stats[StatType.CriticalChance], _stats[StatType.CriticalDamage]);
        _unitWeapon.Attack(attackParams, vec, transform.position.GetVectorInt());
    }

}

public class DefaultUnitMoveState : UnitFSMStateBase
{
    public DefaultUnitMoveState(FSM_Controller<UnitStateType> controller) : base(controller)
    {
    }

    protected override void EnterState()
    {

        
        //transform.DOMove(targetPos, 0.3f)
        //    .OnComplete(HandleEnd);

    }

    protected virtual void HandleEnd()
    {

        controller.InvokeEvent("Move");
        controller.RemoveData("Move");

    }

    private IEnumerator MovementTween(Action endCallback)
    {

        var targetPos = controller.GetData<Vector2>("Move");
        var originPos = controller.transform.position;

        float per = 0;

        while(per < 1)
        {

            per += Time.deltaTime * 3;
            transform.position = Vector3.Lerp(targetPos, originPos, Easing.Get(Ease.OutQuad, per));
            yield return null;

        }

    }

}

public class DefaultUnitFSM : UnitFSMBase
{

    public List<int2> moveRole { get; set; }

    protected override void Awake()
    {

        base.Awake();

        AddState();

    }

    protected virtual void AddState()
    {

        AddState(new DefaultUnitAttackState(this), UnitStateType.Attack);
        AddState(new DefaultUnitMoveState(this), UnitStateType.Move);

    }

    public override void DoAttack()
    {

        ChangeState(UnitStateType.Attack);

    }

    private Vector2 CaculateMovePosition(List<Vector2> ablePos)
    {

        var player = CellObjectManager.Instance.GetCellObjectInstance<PlayerInstance>();
        float min = float.MaxValue;
        Vector2 targetPos = Vector2.zero;

        foreach(var item in ablePos)
        {

            var dest = Vector2.Distance(player.transform.position, item);
            if (dest < min)
            {

                min = dest;
                targetPos = item;

            }

        }

        return targetPos;

    }

    public override Vector2 DoMove(List<Vector2> ablePos, Action endCallback)
    {

        var vec = CaculateMovePosition(ablePos);
        Register("Move", endCallback);
        AddData("Move", vec);
        ChangeState(UnitStateType.Move);
        return vec;

    }

}
