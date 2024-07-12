using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Artifact/HealingItem")]
public class HealingItemSO : ArtifactItemSO
{
    protected override ArtifactType artifactType => ArtifactType.UseImmediately;

    [Space(15)] 
    [SerializeField] private int _healCnt;

    public override void UseArtifact(params object[] args)
    {
        if (!TryParseHandler(OwnerHandler, out IHealingItemHandler healingItemHandler))
        {
            return;
        }
        
        healingItemHandler.Health.AddHp(_healCnt);
        Debug.Log($"{_healCnt} 회복");
    }
}

public interface IHealingItemHandler : IItemHandler
{
    public IHealth Health { get; } 
}