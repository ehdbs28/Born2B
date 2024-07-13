using System.Collections.Generic;
using UnityEngine;

public class ItemSelectPanel : UIComponent
{
    [SerializeField] private ItemInventorySO _inventory;
    private List<ItemSelectUnit> _items;
    
    private Transform _itemParent;

    protected override void Awake()
    {
        base.Awake();
        _items = new List<ItemSelectUnit>();
        _itemParent = transform.Find("UnitParent");
    }

    public override void Appear(Transform parent)
    {
        base.Appear(parent);
        ItemManager.Instance.OnPickItemEvent += ItemSet;
    }

    public override void Disappear(bool poolIn = true)
    {
        ItemManager.Instance.OnPickItemEvent -= ItemSet;
        base.Disappear(poolIn);
    }

    private void ItemSet(ItemSO[] pickItems)
    {
        foreach (var item in _items)
        {
            item.Disappear();
        }
        _items.Clear();

        foreach (var newItemSo in pickItems)
        {
            var itemUI = UIManager.Instance.AppearUI(PoolingItemType.SelectItemUnit, _itemParent) as ItemSelectUnit;
            itemUI.SetItem(newItemSo);
            _items.Add(itemUI);
        }
    }
}
