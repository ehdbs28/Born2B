using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/ConfirmCriticalItem")]
public class ComfirmCriticalItemSO : ArtifactItemSO
{
    protected override ArtifactType artifactType => ArtifactType.Usable;

    public override void UseArtifact(params object[] args)
    {
        if (!TryParseHandler(OwnerHandler, out IStatModifierItemHandler weaponItemHandler))
        {
            return;
        }

        // 여기서 세팅 해주기
        Debug.Log("다음 턴에 치명타 적용");

        EventManager.Instance.RegisterEvent(EventType.OnPlayerTurnOver, HandleAddCriticalChance);

        EventManager.Instance.RegisterEvent(EventType.OnTurnEnded, (x)=>
        {
            weaponItemHandler.Stat.RemoveModifier(StatType.CriticalChance, StatModifierType.Addend, 100);
            EventManager.Instance.UnRegisterEvent(EventType.OnTurnEnded, HandleAddCriticalChance);
        });
    }

    private void HandleAddCriticalChance(params object[] arr)
    {
        if (!TryParseHandler(OwnerHandler, out IStatModifierItemHandler weaponItemHandler))
        {
            return;
        }

        weaponItemHandler.Stat.AddModifier(StatType.CriticalChance, StatModifierType.Addend, 100);
    }
}