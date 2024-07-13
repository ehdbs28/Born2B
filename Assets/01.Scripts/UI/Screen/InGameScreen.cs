using UnityEngine;

public class InGameScreen : UIComponent
{
    public InventoryUI Inventory { get; private set; }
    public PlayerInfoUI PlayerInfo { get; private set; }
    public ArtifactInventoryUI ArtifactInventory { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Inventory = transform.Find("Inventory").GetComponent<InventoryUI>();
        PlayerInfo = transform.Find("PlayerInfo").GetComponent<PlayerInfoUI>();
        ArtifactInventory = transform.Find("Artifacts").GetComponent<ArtifactInventoryUI>();
    }
    
    
}