using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/ReviveItem")]
public class ReviveItemSO : ArtifactItemSO
{
    protected override ArtifactType artifactType => ArtifactType.CallByEvent;
    protected override EventType CallingEventType => EventType.OnPlayerDead;

    public override void UseArtifact(params object[] args)
    {
        Debug.Log("되살아났어용");
    }
}