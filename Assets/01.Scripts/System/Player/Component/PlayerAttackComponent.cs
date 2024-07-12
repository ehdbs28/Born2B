using System;
using UnityEngine;

public class PlayerAttackComponent : PlayerComponent
{
    [SerializeField] AttackInputSO input = null;
    [SerializeField] LayerMask targetLayer = 0;
    private PlayerWeaponComponent weapon = null;
	private Stat attackStat = null;
	private Stat criticalChanceStat = null;
	private Stat criticalDamageStat = null;
    private Camera mainCamera = null;

    private bool active = false;

    public override void Init(PlayerInstance player)
    {
        base.Init(player);

        PlayerStatComponent stat = player.GetPlayerComponent<PlayerStatComponent>();
        attackStat = stat?.GetStat(StatType.Attack);
        criticalChanceStat = stat?.GetStat(StatType.CriticalChance);
        criticalDamageStat = stat?.GetStat(StatType.CriticalDamage);

        weapon = player.GetPlayerComponent<PlayerWeaponComponent>();
        mainCamera = Camera.main;

        input.OnAttackEvent += HandleAttack;
        EventManager.Instance.RegisterEvent(EventType.OnTurnChanged, HandleTurnChanged);
    }

    public override void Release()
    {
        base.Release();
        input.OnAttackEvent -= HandleAttack;
        // EventManager.Instance.UnRegisterEvent(EventType.OnTurnChanged, HandleTurnChanged);
    }

    private void Update()
    {

        if(active == false)
            return;

        Vector2Int position = player.transform.position.GetVectorInt();
        Vector2Int point = mainCamera.ScreenToWorldPoint(input.ScreenPosition).GetVectorInt();
        weapon.RotateWeapon(position, point);

        weapon.ClearDraw();
        weapon.DrawRange();
    }

    private void HandleAttack()
    {
        Vector2Int inputPosition = mainCamera.ScreenToWorldPoint(input.ScreenPosition).GetVectorInt();
        AttackParams attackParams = new AttackParams(attackStat, criticalChanceStat, criticalDamageStat, targetLayer);
        weapon.Attack(inputPosition, attackParams);
        
        // 아티팩트 등으로 공격 회수가 늘어나게 되면 이 부분 수정해야됨
        // 혹은 여기는 놔두고 PlayerAttackTurn 자체를 여러번 하는 것도 괜찮음.
        active = false;
        weapon.ClearDraw();
        TurnManager.Instance.EndCurrentTurn();
    }

    private void HandleTurnChanged(params object[] args)
    {

        if (FlowManager.Instance.CurrentCycle == EventType.OnBattleFinish)
            return;

        // TurnObjectSO prevTurn = args[0] as TurnObjectSO;
        TurnType nextTurn = (TurnType)args[1];

        if(nextTurn != TurnType.PlayerAttack)
            return;

        active = true;
    }
}
