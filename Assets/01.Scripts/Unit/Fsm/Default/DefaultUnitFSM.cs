using DG.Tweening;
using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DefaultUnitAttackState : UnitFSMStateBase
{
    public DefaultUnitAttackState(FSM_Controller<UnitStateType> controller) : base(controller)
    {
    }

    protected override void EnterState()
    {

        var vec = transform.position.GetVectorInt();
        var player = CellObjectManager.Instance.GetCellObjectInstance<PlayerInstance>();
        var dir = player.transform.position - transform.position;
        var ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _unitWeapon.RotateAttackRange(ang);
        // _unitWeapon.Attack(0, vec, transform.position.GetVectorInt());

        // 대원이에게...
        // 대원아. 너의 코드를 수정하게 되었어. 우선 미안하다는 말을 먼저 남길게.
        // 수정하게 된 이유는 Weapon.Attack의 매개변수를 변경했기 때문이야.
        // damage만을 넘겼던 걸 치명타 확률과, 치명타 데미지를 모두 넘기고자 AttackParams라는 걸 만들었어.
        // 혹시 이 변경이 너의 기분을 상하게 했다면 사과할게. 미안해.
        // 그럼 AttackParams에 원하는 필드를 채워서 공격을 진행시키면 돼.
        //                                                  세훈이가...
        //                                              2024년 7월 11일
        AttackParams attackParams = new AttackParams(0, 0, 0);
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

        var targetPos = controller.GetData<Vector2>("Move");
        transform.DOMove(targetPos, 0.3f)
            .OnComplete(HandleEnd);

    }

    protected virtual void HandleEnd()
    {

        controller.InvokeEvent("Move");
        controller.RemoveData("Move");

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
