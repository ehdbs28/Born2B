using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSelectUnit : UIButton
{
    [SerializeField] private ItemInventorySO _inventory;
    
    private Image _itemImage;
    private TextMeshProUGUI _nameText;
    
    private ItemSO _holdingItemSo;
    private ItemInfoPopup _infoPopup;
    
    public event Action OnSelectEvent;

    protected override void Awake()
    {
        base.Awake();

        _itemImage = transform.Find("ItemImage").GetComponent<Image>();
        _nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
    }

    public void SetItem(ItemSO newItem)
    {
        _itemImage.sprite = newItem.ItemIcon;
        _nameText.text = newItem.ItemName;

        _holdingItemSo = newItem;
    }

    public void SelectItem(PointerEventData eventData)
    {
        if (_holdingItemSo == null)
        {
            return;
        }

        if (_inventory.AddItem(_holdingItemSo))
        {
            FlowManager.Instance.NextCycle();
            return;
        }
        
        // 아이템이 모종의 이유로 들어가지 못하였을 때
        FlowManager.Instance.NextCycle();
        
        OnSelectEvent?.Invoke();
        OnSelectEvent = null;
    }
    
    public void ShowInfoUI(PointerEventData eventData)
    {
        if (_infoPopup != null || _holdingItemSo == null)
        {
            return;
        }
        
        _infoPopup = UIManager.Instance.AppearUI(PoolingItemType.ItemInfoPopup) as ItemInfoPopup;
        _infoPopup.Init(_holdingItemSo);
    }

    public void UnShowInfoUI(PointerEventData eventData)
    {
        if (_infoPopup == null)
        {
            return;
        }
        
        _infoPopup.Disappear();
        _infoPopup = null;
    }

    public void UpdateInfoUI(PointerEventData eventData)
    {
        if (_infoPopup == null)
        {
            return;
        }

        var mousePosition = eventData.position;
        _infoPopup.SetPosition(mousePosition);
    }
}