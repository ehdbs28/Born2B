using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/ConfirmCriticalItem")]
public class ComfirmCriticalItemSO : ArtifactItemSO
{
    protected override ArtifactType artifactType => ArtifactType.Usable;
    private IStatModifierItemHandler _weaponItemHandler;

    public override void UseArtifact(params object[] args)
    {
        if (!TryParseHandler(OwnerHandler, out IStatModifierItemHandler weaponItemHandler))
        {
            return;
        }

        // 여기서 세팅 해주기
        _weaponItemHandler = weaponItemHandler;

        EventManager.Instance.RegisterEvent(EventType.OnTurnEnded, HandleAddCriticalChance);
        Debug.Log("다음 턴에 치명타 적용");
        EventManager.Instance.RegisterEvent(EventType.OnPlayerAttacked, HandleRemoveCriticalChance);

    }

    private void HandleAddCriticalChance(params object[] arr)
    {
        _weaponItemHandler.Stat.AddModifier(StatType.CriticalChance, StatModifierType.Addend, 100);
        EventManager.Instance.UnRegisterEvent(EventType.OnTurnEnded, HandleAddCriticalChance);
    }

    private void HandleRemoveCriticalChance(params object[] arr)
    {
        _weaponItemHandler.Stat.RemoveModifier(StatType.CriticalChance, StatModifierType.Addend, 100);
        EventManager.Instance.UnRegisterEvent(EventType.OnPlayerAttacked, HandleRemoveCriticalChance);
    }
}