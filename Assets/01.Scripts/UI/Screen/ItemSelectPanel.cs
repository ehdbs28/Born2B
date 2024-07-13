using System.Collections.Generic;
using UnityEngine;

public class ItemSelectPanel : UIComponent
{
    private List<ItemSelectUnit> _items;
    
    private Transform _itemParent;

    protected override void Awake()
    {
        base.Awake();
        _items = new List<ItemSelectUnit>();
        _itemParent = transform.Find("UnitParent");
    }

    public override void Disappear(bool poolIn = true)
    {
        foreach (var item in _items)
        {
            item.Disappear();
        }
        _items.Clear();
        base.Disappear(poolIn);
    }

    public void ItemSet(ItemSO[] pickItems)
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
            itemUI.OnSelectEvent += () => Disappear();
            _items.Add(itemUI);
        }
    }
}
