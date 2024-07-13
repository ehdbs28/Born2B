using UnityEngine;

public class InGameScreen : UIComponent
{
    public InventoryUI Inventory { get; private set; }
    public PlayerInfoUI PlayerInfo { get; private set; }
    public ArtifactInventoryUI ArtifactInventory { get; private set; }
    public TurnSkipUI SkipPanel { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Inventory = transform.Find("Inventory").GetComponent<InventoryUI>();
        PlayerInfo = transform.Find("PlayerInfo").GetComponent<PlayerInfoUI>();
        ArtifactInventory = transform.Find("Artifacts").GetComponent<ArtifactInventoryUI>();
        SkipPanel = transform.Find("SkipButton").GetComponent<TurnSkipUI>();
    }

    public override void Appear(Transform parent)
    {
        base.Appear(parent);
        
        Inventory.Init();
        ArtifactInventory.Init();
        SkipPanel.Init();
    }

    public override void Disappear(bool poolIn = true)
    {
        base.Disappear(poolIn);
        
        Inventory.Release();
        ArtifactInventory.Release();
        SkipPanel.Release();
    }
}