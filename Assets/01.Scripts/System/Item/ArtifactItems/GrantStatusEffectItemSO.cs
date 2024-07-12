using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/GrantStatusEffectItem")]
public class GrantStatusEffectItemSO : ArtifactItemSO
{
    protected override ArtifactType artifactType => ArtifactType.Usable;

    [SerializeField] List<StatusEffectSlot> statusEffects;
    
    public override void UseArtifact(params object[] args)
    {
        if (!TryParseHandler(OwnerHandler, out IWeaponArtifactItemHandler weaponItemHandler))
            return;
        
        statusEffects.ForEach(weaponItemHandler.Weapon.AddDisposableStatusEffect);
    }
}