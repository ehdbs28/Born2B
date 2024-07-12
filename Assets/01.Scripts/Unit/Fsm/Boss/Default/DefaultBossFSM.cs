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

        private UnitWeaponController _meleeWeapon;
        private UnitWeaponController _hitscanWeapon;

        public BossAttackState(FSM_Controller<UnitStateType> controller) : base(controller)
        {

            var container = transform.Find("WeaponContainer");


        }

        protected override void EnterState()
        {

            _meleeWeapon = transform.Find("Melee").GetComponent<UnitWeaponController>();
            _hitscanWeapon = transform.Find("Range").GetComponent<UnitWeaponController>();
            var vec = transform.position.GetVectorInt();
            var player = CellObjectManager.Instance.GetCellObjectInstance<PlayerInstance>();
            var dist = Vector2.Distance(vec, player.transform.position);
            var wep = dist < 3f ? _meleeWeapon : _hitscanWeapon;

            wep.Attack(vec);

        }

    }

    public class BossMoveState : DefaultUnitMoveState
    {

        private UnitWeaponController _melee;

        public BossMoveState(FSM_Controller<UnitStateType> controller) : base(controller)
        {

            var container = transform.Find("WeaponContainer");
            
        }

        protected override void HandleEnd()
        {

            _melee = transform.Find("Melee").GetComponent<UnitWeaponController>();

            StartCoroutine(WaitEnd());

            var vec = transform.position.GetVectorInt();
            _melee.Attack(vec);

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
