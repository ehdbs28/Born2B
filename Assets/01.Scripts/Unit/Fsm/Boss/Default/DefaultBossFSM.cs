//using DG.Tweening;
using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class DefaultBossFSM : DefaultUnitFSM
{

    public class BossAttackState : UnitFSMStateBase
    {

        private MeleeWeapon _meleeWeapon;
        private HitscanWeapon _hitscanWeapon;

        public BossAttackState(FSM_Controller<UnitStateType> controller) : base(controller)
        {

            var ins = GetComponent<CellObjectInstance>();
            var container = transform.Find("WeaponContainer");
            _meleeWeapon = container.GetComponent<MeleeWeapon>();
            _hitscanWeapon = container.GetComponent<HitscanWeapon>();

            _meleeWeapon.Init(ins);
            _hitscanWeapon.Init(ins);

        }

        protected override void EnterState()
        {

            var vec = transform.position.GetVectorInt();
            var player = CellObjectManager.Instance.GetCellObjectInstance<PlayerInstance>();
            var dir = player.transform.position - transform.position;
            var ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Weapon wep = Random.value > 0.5f ? _meleeWeapon : _hitscanWeapon;

            wep.RotateAttackRange(ang);

            var param = new AttackParams(_stats[StatType.Attack], _stats[StatType.CriticalChance], _stats[StatType.CriticalDamage], 0);

            wep.Attack(param, vec, transform.position.GetVectorInt());

        }

    }

    public class BossMoveState : DefaultUnitMoveState
    {

        private MeleeWeapon _melee;

        public BossMoveState(FSM_Controller<UnitStateType> controller) : base(controller)
        {

            var ins = GetComponent<CellObjectInstance>();
            var container = transform.Find("WeaponContainer");
            _melee = container.GetComponent<MeleeWeapon>();

            _melee.Init(ins);

        }

        protected override void HandleEnd()
        {

            StartCoroutine(WaitEnd());

            var vec = transform.position.GetVectorInt();
            var player = CellObjectManager.Instance.GetCellObjectInstance<PlayerInstance>();
            var dir = player.transform.position - transform.position;
            var ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            _melee.RotateAttackRange(ang);
            var param = new AttackParams(_stats[StatType.Attack], _stats[StatType.CriticalChance], _stats[StatType.CriticalDamage], 0);
            _melee.Attack(param, vec, transform.position.GetVectorInt());

        }

        private IEnumerator WaitEnd()
        {

            yield return new WaitForSeconds(1f);
            base.HandleEnd();

        }

    }

    protected override void AddState()
    {

        AddState(new BossAttackState(this), UnitStateType.Attack);
        AddState(new BossMoveState(this), UnitStateType.Move);

    }

}
