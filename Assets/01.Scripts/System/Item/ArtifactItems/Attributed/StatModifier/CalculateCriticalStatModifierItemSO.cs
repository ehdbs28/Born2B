using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/CalculateCriticalStatModifierItem")]
public class CalculateCriticalStatModifierItemSO : AttributedCallByEventItemSO
{
    [SerializeField] List<StatModifierSlot> basicModifiers = new List<StatModifierSlot>();
    [SerializeField] List<StatModifierSlot> criticalModifiers = new List<StatModifierSlot>();
    [SerializeField] List<StatModifierSlot> uncriticalModifiers = new List<StatModifierSlot>();

    protected override EventType CallingEventType => EventType.OnPlayerAttacked;

    private Weapon weapon = null;
    private bool critical = false;

    public override void Execute(IItemHandler handler)
    {
        base.Execute(handler);
        AddModifier(basicModifiers);
        EventManager.Instance.RegisterEvent(EventType.OnPlayerWeaponChanged, HandlePlayerWeaponChanged);
    }

    public override void UseArtifact(params object[] args)
    {
        if(weapon == null)
            return;

        RemoveModifier(critical ? criticalModifiers : uncriticalModifiers);
    }

    public override void Unexecute(IItemHandler handler)
    {
        Release();
        RemoveModifier(basicModifiers);
        EventManager.Instance.UnRegisterEvent(EventType.OnPlayerWeaponChanged, HandlePlayerWeaponChanged);
        base.Unexecute(handler);
    }

    private void HandleCalculateCiritical(bool critical)
    {
        AddModifier(critical ? criticalModifiers : uncriticalModifiers);
        this.critical = critical;
    }

    private void AddModifier(List<StatModifierSlot> modifier)
    {
        if (TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;
        modifier.ForEach(statModifierHandler.Stat.AddModifier);
    }

    private void RemoveModifier(List<StatModifierSlot> modifier)
    {
        if (TryParseHandler<IStatModifierItemHandler>(OwnerHandler, out IStatModifierItemHandler statModifierHandler) == false)
            return;
        modifier.ForEach(statModifierHandler.Stat.RemoveModifier);
    }

    private void Init()
    {
        if (TryParseHandler<IWeaponArtifactItemHandler>(OwnerHandler, out IWeaponArtifactItemHandler weaponHandler) == false)
            return;

        weapon = weaponHandler.CurrentWeapon;
        if(weapon != null)
            weapon.OnCaculateCriticalEvent += HandleCalculateCiritical;
    }

    private void Release()
    {
        if (TryParseHandler<IWeaponArtifactItemHandler>(OwnerHandler, out IWeaponArtifactItemHandler weaponHandler) == false)
            return;

        if (weapon != null && weapon == weaponHandler.CurrentWeapon)
            weapon.OnCaculateCriticalEvent -= HandleCalculateCiritical;
    }

    private void HandlePlayerWeaponChanged(object[] args)
    {
        Release();
        Init();
    }
}
