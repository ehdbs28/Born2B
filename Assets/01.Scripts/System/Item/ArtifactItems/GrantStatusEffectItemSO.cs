using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/GrantStatusEffectItem")]
public class GrantStatusEffectItemSO : ArtifactItemSO
{
    protected override ArtifactType artifactType => ArtifactType.Usable;

    [SerializeField] List<StatusEffectSlot> statusEffects;
    
    public override void UseArtifact(params object[] args)
    {
        if (!TryParseHandler(OwnerHandler, out IGrantStatusEffectItemHandler grantStatusEffectItemHandler))
        {
            return;
        }
        
        statusEffects.ForEach(grantStatusEffectItemHandler.WeaponComponent.AddDisposableStatusEffect);
    }
}

public interface IGrantStatusEffectItemHandler : IItemHandler
{
    public Weapon WeaponComponent { get; }
}