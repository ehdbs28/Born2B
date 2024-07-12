using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryComponent : PlayerComponent
{
	[SerializeField] ItemInventorySO inventoryData = null;

    public UnityEvent<ItemInventorySO, ItemSO> OnItemAddedEvent;
    public UnityEvent<ItemInventorySO, ItemSO> OnItemRemovedEvent;

    public override void Init(PlayerInstance player)
    {
        base.Init(player);

        inventoryData.Init(player);
        inventoryData.OnItemAddedEvent += HandleItemAdded;
        inventoryData.OnItemRemovedEvent += HandleItemRemoved;
    }

    private void HandleItemAdded(ItemSO item) => OnItemAddedEvent?.Invoke(inventoryData, item);
    private void HandleItemRemoved(ItemSO item) => OnItemRemovedEvent?.Invoke(inventoryData, item);
}
