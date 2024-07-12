using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/ConfirmCriticalItem")]
public class ConfirmCriticalItemSO : ArtifactItemSO
{
    protected override ArtifactType artifactType => ArtifactType.Usable;
    
    public override void UseArtifact(params object[] args)
    {
        if (!TryParseHandler(OwnerHandler, out IWeaponArtifactItemHandler weaponItemHandler))
        {
            return;
        }
        
        // confirmCriticalItemHandler.WeaponComponent.WeaponData
        // 여기서 세팅 해주기
        Debug.Log("다음 턴에 치명타 적용");
    }
}