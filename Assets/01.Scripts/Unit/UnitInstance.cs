//using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UnitInstance : CellObjectInstance, IMovementable, IAttackable, IHitable
{
    [field: SerializeField] public List<int2> moveRole { get; set; }
    [SerializeField] private AudioData _data;
    protected UnitFSMBase _unitFSMBase;
    protected UnitStatContainer _unitStatContainer;
    protected UnitWeaponController _weaponController;
    protected UnitHealth _health;

    public Vector2Int Position => transform.position.GetVectorInt();

    [SerializeField] UnityEvent onMoveEvent = new UnityEvent();
    [SerializeField] UnityEvent onAttackEvent = new UnityEvent();
    [SerializeField] UnityEvent onHitEvent = new UnityEvent();
    [SerializeField] UnityEvent onDeadEvent = new UnityEvent();



    protected override void Awake()
    {
        base.Awake();

        _collider = GetComponent<Collider2D>();
        _unitStatContainer = GetComponent<UnitStatContainer>();
        _unitFSMBase = GetComponent<UnitFSMBase>();
        _weaponController = GetComponent<UnitWeaponController>();
        _health = GetComponent<UnitHealth>();
    }



    public override void Init(CellObjectSO so)
    {
        
        base.Init(so);
        var casted = so as UnitDataSO;
        casted.Init();

        casted.health = _health;
        casted.statusController = GetComponent<StatusController>();
        _unitStatContainer.Init(casted.Stat);
        _weaponController.Init(casted.weaponItem, this);
        _health.ResetHp();

        moveRole = casted.movementRole;

    }

    public bool Hit(CellObjectInstance attackObject, float damage, bool critical)
    {

        if (attackObject is UnitInstance) return false;

        bool die = _health.CurrentHp <= 0;
        EventManager.Instance.PublishEvent(EventType.OnUnitDamaged, this, die);
        onHitEvent?.Invoke();

        _health.ReduceHp((int)damage);

        if(die)
        {
            
            onDeadEvent?.Invoke();
            Die();

        }

        return true;
    }

    public void Attack()
    {

        _unitFSMBase.DoAttack();
        onAttackEvent?.Invoke();

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

    protected override void Update()
    {

        base.Update();

        var player = CellObjectManager.Instance.GetCellObjectInstance<PlayerInstance>();

        if(player != null)
        {

            var c = Vector3.Cross(Vector3.up, player.transform.position - transform.position);

            _renderer.flipX = c.z > 0;

        }

    }

    public Vector2 Move(List<Vector2> targetPositions, Action endCallback)
    {

        AudioManager.Instance.PlayAudio(_data);
        return _unitFSMBase.DoMove(targetPositions, () => {
            endCallback?.Invoke();
            onMoveEvent?.Invoke();
        });
    }

}
