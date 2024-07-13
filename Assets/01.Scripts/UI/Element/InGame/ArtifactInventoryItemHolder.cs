using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArtifactInventoryItemHolder : UIButton
{
    private ArtifactItemSO _holdingItemSo;
    private ItemInfoPopup _infoPopup;

    private Transform _itemBorder;
    private Image _innerImage;

    protected override void Awake()
    {
        base.Awake();

        _itemBorder = transform.Find("ArtifactItem");
        _innerImage = _itemBorder.Find("Image").GetComponent<Image>();
    }

    public void SetItem(ArtifactItemSO newItem)
    {
        if (newItem == null)
        {
            _itemBorder.gameObject.SetActive(false);
            return;
        }

        _itemBorder.gameObject.SetActive(true);
        _innerImage.sprite = newItem.ItemIcon;

        _holdingItemSo = newItem;
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