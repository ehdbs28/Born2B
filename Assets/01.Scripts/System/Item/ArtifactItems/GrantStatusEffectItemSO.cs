using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/GrantStatusEffectItem")]
public class GrantStatusEffectItemSO : ArtifactItemSO
{
    protected override ArtifactType ArtifactType => ArtifactType.Usable;

    [SerializeField] private StatusType _grantStatusType;
    private StatusType _prevWeaponStatusType;
    
    public override void UseArtifact(params object[] args)
    {
        if (!TryParseHandler(OwnerHandler, out IGrantStatusEffectItemHandler grantStatusEffectItemHandler))
        {
            return;
        }

        var weapon = grantStatusEffectItemHandler.WeaponComponent;
        _prevWeaponStatusType = weapon.WeaponData.EffectedStatusType;
        weapon.WeaponData.EffectedStatusType = _grantStatusType;
        
        EventManager.Instance.RegisterEvent(EventType.OnTurnEnded, RestorePrevStatusType);
        
        // grantStatusEffectItemHandler.WeaponComponent.WeaponData
        // 여기서 세팅 해주기
        Debug.Log($"다음 턴에 {_grantStatusType} 적용");
    }

    private void RestorePrevStatusType(params object[] args)
    {
        var weapon = args[0] as Weapon;
        weapon.WeaponData.EffectedStatusType = _prevWeaponStatusType;
        EventManager.Instance.UnRegisterEvent(EventType.OnTurnEnded, RestorePrevStatusType);
    }
}

public interface IGrantStatusEffectItemHandler : IItemHandler
{
    public Weapon WeaponComponent { get; }
}