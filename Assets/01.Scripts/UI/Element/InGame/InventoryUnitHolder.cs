using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUnitHolder : UIButton
{
    [SerializeField] private ItemInventorySO _itemInventory;
    
    [SerializeField] private Sprite _setUpSprite;
    [SerializeField] private Sprite _nonSetUpSprite;

    private ArtifactItemSO _holdingItemSo;
    private ItemInfoPopup _infoPopup;

    private Image _borderImage;
    private Image _innerImage;

    protected override void Awake()
    {
        base.Awake();

        _borderImage = GetComponent<Image>();
        _innerImage = transform.Find("Item").GetComponent<Image>();
    }

    public void SetItem(ArtifactItemSO newItem)
    {
        if (newItem == null)
        {
            _borderImage.color = new Color(1f, 1f, 1f, 75f / 255f);
            _innerImage.color = new Color(1f, 1f, 1f, 0f);
            _borderImage.sprite = _nonSetUpSprite;
            _innerImage.enabled = false;
            return;
        }

        _borderImage.color = Color.white;
        _borderImage.sprite = _setUpSprite;
        
        _innerImage.color = new Color(1f, 1f, 1f, 1f);

        _innerImage.enabled = true;
        _innerImage.sprite = newItem.ItemIcon;

        _holdingItemSo = newItem;
    }

    public void UseItem()
    {
        if (_holdingItemSo == null)
        {
            return;
        }
        
        _holdingItemSo.UseArtifact();
        _itemInventory.RemoveItem(_holdingItemSo);
        UnShowInfoUI(null);
        SetItem(null);
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